#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug
//		Date:			7/20/2018 10:00:36 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Debugger_For_Unity {

    /// <summary>
    /// Partial Class
    /// </summary>
    public partial class Debugger
    {
        [Serializable]
        private sealed class Debug : IWindow
        {
            #region  Attributes and Properties
            public Debugger Debugger { get; set; }

            /// <summary>
            /// Private Members
            /// </summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            // button debug
            [SerializeField]
            private int m_numberOfButtonsPerLine = 5;
            private int m_debugButtonMethodNum = 0;
            private Dictionary<int, string> m_debugButtonMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugButtonDescritionDict = new Dictionary<int, string>();

            // select debug
            private int m_debugSelectMethodNum = 0;
            private Dictionary<int, string> m_debugSelectMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugSelectDescritionDict = new Dictionary<int, string>();
            private string[] m_debugSelectDescriptionArray;
            private int m_selectIndexNumber;
            private bool m_selectShow = false;
            private Vector2 scrollViewSelectVector = Vector2.zero;

            // code debug
            private int m_debugCodeMethodNum = 0;
            private Dictionary<int, string> m_debugCodeMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeCustomCodeDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeDescritionDict = new Dictionary<int, string>();
            private string[] m_debugCodeDescriptionArray;
            private string[] m_debugCodeCustomCodeArray;
            private string[] m_matchedCodeArray = { };
            private bool m_showCodeScroll = false;
            private Vector2 scrollViewCodeVector = Vector2.zero;
            private string DisplayedCode = "";
            private string LastDisplayedCode = "";

            #endregion

            #region Private Methods
            /// <summary>
            /// refresh the matched input code to give tips
            /// </summary>
            private void RefreshCodeList()
            {
                Dictionary<int, string> refreshCode = new Dictionary<int, string>();
                int index = 0;
                foreach (string s in m_debugCodeCustomCodeArray)
                {
                    if(s.StartsWith(DisplayedCode, System.StringComparison.CurrentCultureIgnoreCase) || s.Contains(DisplayedCode))
                    {
                        refreshCode[index] = s;
                        index++;
                    }
                }
                m_matchedCodeArray = refreshCode.Values.ToArray();
            }

            /// <summary>
            /// check the code, prepare to invoke
            /// </summary>
            /// <param name="code"></param>
            private void DealWithCustomCode(string code)
            {
                code = code.Trim();
                string[] ss = System.Text.RegularExpressions.Regex.Split(code, @"\s+");
                int codeSplitParamNum = ss.Length;
                if (codeSplitParamNum <= 0 || ss[0] == "")
                {
                    return;
                }
                if (!m_debugCodeCustomCodeDict.Values.Contains(ss[0]))
                {
                    UnityEngine.Debug.Log("No Such Code");
                    return;
                }
                int key = m_debugCodeCustomCodeDict.FirstOrDefault(x => x.Value == ss[0]).Key;
                MethodInfo method = typeof(Debugger).GetMethod(m_debugCodeMethodDict[key]);
                if (codeSplitParamNum != (method.GetParameters().Length + 1))
                {
                    UnityEngine.Debug.Log("Wrong Code, Reason: Number of Parameters");
                    return;
                }
                string[] param = new string[codeSplitParamNum - 1];
                for (int i = 0; i < (codeSplitParamNum - 1); i++)
                {
                    param[i] = ss[i + 1];
                }
                UnityEngine.Debug.Log("Apply Code: [" + code + "] Successful.");
                method.Invoke(Debugger, param);
            }

            private void DealWithSelect(int index)
            {
                MethodInfo method = typeof(Debugger).GetMethod(m_debugSelectMethodDict[index]);
                if(method.GetParameters().Length > 0)
                {
                    UnityEngine.Debug.LogWarning(method.Name + " Method has unwanted parameters");
                }
                method.Invoke(Debugger, null);
            }

            private void DealWithButton(int index)
            {
                MethodInfo method = typeof(Debugger).GetMethod(m_debugButtonMethodDict[index]);
                if (method.GetParameters().Length > 0)
                {
                    UnityEngine.Debug.LogWarning(method.Name + " Method has unwanted parameters");
                }
                method.Invoke(Debugger, null);
            }
            #endregion


            #region Interface Public Methods
            public void OnWindowAwake(params object[] args)
            {
                Type type = typeof(Debugger);

                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (Attribute attr in method.GetCustomAttributes(true))
                    {
                        if (attr is DebuggerButtonDebugAttribute)
                        {
                            m_debugButtonMethodDict.Add(m_debugButtonMethodNum, method.Name);
                            m_debugButtonDescritionDict.Add(m_debugButtonMethodNum, ((DebuggerButtonDebugAttribute)attr).Description);
                            m_debugButtonMethodNum++;
                        }
                        if (attr is DebuggerSelectDebugAttribute)
                        {
                            m_debugSelectMethodDict.Add(m_debugSelectMethodNum, method.Name);
                            m_debugSelectDescritionDict.Add(m_debugSelectMethodNum, ((DebuggerSelectDebugAttribute)attr).Description);
                            m_debugSelectMethodNum++;
                        }
                        if (attr is DebuggerCodeDebugAttribute)
                        {
                            m_debugCodeMethodDict.Add(m_debugCodeMethodNum, method.Name);
                            m_debugCodeCustomCodeDict.Add(m_debugCodeMethodNum, ((DebuggerCodeDebugAttribute)attr).CustomCode);
                            m_debugCodeDescritionDict.Add(m_debugCodeMethodNum, ((DebuggerCodeDebugAttribute)attr).Description);
                            m_debugCodeMethodNum++;
                        }
                    }
                }

                m_debugSelectDescriptionArray = m_debugSelectDescritionDict.Values.ToArray();
                m_debugCodeDescriptionArray = m_debugCodeDescritionDict.Values.ToArray();
                m_debugCodeCustomCodeArray = m_debugCodeCustomCodeDict.Values.ToArray();
            }


            public void OnWindowDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    #region Code Debug
                    //
                    // Code Debug
                    //

                    // title and input field

                    GUILayout.Label("<b>Code</b>", GUILayout.Height(30f));

                    //GUILayout.BeginVertical("box");
                    //{
                        DisplayedCode = GUILayout.TextField(DisplayedCode, 100, GUILayout.Height(25f));
                    //}
                    //GUILayout.EndVertical();

                    if (LastDisplayedCode != DisplayedCode)
                    {
                        m_showCodeScroll = true;
                        LastDisplayedCode = DisplayedCode;
                        RefreshCodeList();
                    }


                    // dropdown tips

                    if (DisplayedCode != "" && m_showCodeScroll != false)
                    {
                        Rect codeDropDownRect = new Rect(0, 0, 200, 20);

                        scrollViewCodeVector = GUI.BeginScrollView(new Rect(150, 60, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))), scrollViewCodeVector, new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))));

                        GUI.Box(new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))), "");

                        for (int index = 0; index < m_matchedCodeArray.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), codeDropDownRect.width, 25), ""))
                            {
                                DisplayedCode = m_matchedCodeArray[index];
                                m_showCodeScroll = false;
                                LastDisplayedCode = DisplayedCode;
                            }
                            GUI.Label(new Rect(5, (index * 25), codeDropDownRect.width, 25), "<b>" + m_matchedCodeArray[index] + "</b>");
                        }
                        GUI.EndScrollView();
                    }

                    // clear and enter button

                    //GUILayout.BeginVertical("box");
                    //{
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("<b>Clear</b>", GUILayout.Width(100f), GUILayout.Height(30f)))
                        {
                            DisplayedCode = "";
                        }
                        if (GUILayout.Button("<b>Enter</b>", GUILayout.Height(30f)))
                        {
                            DealWithCustomCode(DisplayedCode);
                            TextEditor textEditor = new TextEditor();
                            textEditor.text = DisplayedCode;
                            textEditor.OnFocus();
                            textEditor.Copy();
                            DisplayedCode = "";
                        }
                    }
                    GUILayout.EndHorizontal();
                    //}
                    //GUILayout.EndVertical();
                    #endregion

                    #region Select Debug
                    //
                    // Select Debug
                    //

                    // title and select button

                    GUILayout.Label("<b>Select</b>");

                    if (GUILayout.Button(""))
                    {
                        if (!m_selectShow)
                        {
                            m_selectShow = true;
                        }
                        else
                        {
                            m_selectShow = false;
                        }
                    }

                    // drawdown selections

                    Rect selectDropDownRect = new Rect(10, 125, 400, 100);
                    if (m_selectShow)
                    {
                        scrollViewSelectVector = GUI.BeginScrollView(new Rect(selectDropDownRect.x, selectDropDownRect.y + 20, selectDropDownRect.width, selectDropDownRect.height), scrollViewSelectVector, new Rect(0, 0, selectDropDownRect.width - 20, Mathf.Max(selectDropDownRect.height, (m_debugSelectDescriptionArray.Length * 25))));

                        GUI.Box(new Rect(0, 0, selectDropDownRect.width, Mathf.Max(selectDropDownRect.height, (m_debugSelectDescriptionArray.Length * 25))), "");

                        for (int index = 0; index < m_debugSelectDescriptionArray.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), selectDropDownRect.width, 25), ""))
                            {
                                m_selectShow = false;
                                m_selectIndexNumber = index;
                            }

                            GUI.Label(new Rect(selectDropDownRect.x, (index * 25), selectDropDownRect.width, 25), m_debugSelectDescriptionArray[index]);
                        }
                        GUI.EndScrollView();
                    }
                    else
                    {
                        GUI.Label(new Rect(selectDropDownRect.x, selectDropDownRect.y, selectDropDownRect.width, 25), m_debugSelectDescriptionArray[m_selectIndexNumber]);
                    }

                    // enter button

                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("<b>Enter</b>", GUILayout.Height(30f)))
                        {
                            DealWithSelect(m_selectIndexNumber);

                        }
                    }
                    GUILayout.EndVertical();
                    #endregion

                    #region Button Debug
                    //
                    // Button Debug
                    //

                    GUILayout.Label("<b>Button</b>");
                    int lines;
                    if (m_debugButtonMethodNum % m_numberOfButtonsPerLine > 0)
                    {
                        lines = m_debugButtonMethodNum / m_numberOfButtonsPerLine + 1;
                    }
                    else
                    {
                        lines = m_debugButtonMethodNum / m_numberOfButtonsPerLine;
                    }
                     

                    for (int i = 0; i < lines; i++)
                    {
                        GUILayout.BeginVertical("box");
                        {
                            GUILayout.BeginHorizontal();
                            {
                                for (int j = 0; j < m_numberOfButtonsPerLine; j++)
                                {
                                    if ((m_numberOfButtonsPerLine * i + j) < m_debugButtonMethodNum)
                                    {
                                        if (GUILayout.Button(m_debugButtonDescritionDict[m_numberOfButtonsPerLine * i + j], GUILayout.Width(100f), GUILayout.Height(30f)))
                                        {
                                            DealWithButton(m_numberOfButtonsPerLine * i + j);
                                        }
                                        GUILayout.Label("", GUILayout.Width(10f));
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    #endregion
                }
                GUILayout.EndScrollView();
            }

            public void OnWindowDestroy()
            {

            }

            public void OnWindowEnter()
            {
                
            }

            public void OnWindowExit()
            {
                
            }

            public void OnWindowStay(float deltaTime, float unscaledDeltaTime)
            {
                
            }
            #endregion

        }
    }
}