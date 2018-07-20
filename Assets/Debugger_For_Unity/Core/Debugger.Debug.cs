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
            /// <summary>
            /// Public Members
            /// </summary>
            public string DisplayedCode = "";

            public string LastDisplayedCode = "";

            bool m_showCodeScroll = false;
            /// <summary>
            /// Properties
            /// </summary>

            /// <summary>
            /// Protected Members
            /// </summary>

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

            // code debug
            private int m_debugCodeMethodNum = 0;
            private Dictionary<int, string> m_debugCodeMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeCustomCodeDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeDescritionDict = new Dictionary<int, string>();
            private string[] m_debugCodeDescriptionArray;

            private int m_selectIndexNumber;
            private bool m_selectShow = false;

            private Vector2 scrollViewCodeVector = Vector2.zero;
            private Vector2 scrollViewSelectVector = Vector2.zero;

            //test, temp
            private string[] m_codeList = { "aaa", "bbb", "ccc", "ddd"};
            private string[] list = { "Drop_Down_Menu", "Drop_Down_Menu2", "Drop_Down_Menu33", "Drop_Down_Menu44", "Drop_Down_Menu2", "Drop_Down_Menu33", "Drop_Down_Menu44", "Drop_Down_Menu2", "Drop_Down_Menu33", "Drop_Down_Menu44", "Drop_Down_Menu2", "Drop_Down_Menu33", "Drop_Down_Menu44", "Drop_Down_Menu2", "Drop_Down_Menu33", "Drop_Down_Menu44" };
            #endregion


            #region Engine Methods

            #endregion


            #region Public Methods

            #endregion


            #region Protected Methods

            #endregion


            #region Private Methods

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
            }

            public void OnWindowDestroy()
            {
                
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
                    }


                    // dropdown tips

                    if (DisplayedCode != "" && m_showCodeScroll != false)
                    {
                        Rect codeDropDownRect = new Rect(0, 0, 200, 20);

                        scrollViewCodeVector = GUI.BeginScrollView(new Rect(150, 60, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_codeList.Length * 25))), scrollViewCodeVector, new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_codeList.Length * 25))));

                        GUI.Box(new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_codeList.Length * 25))), "");

                        for (int index = 0; index < m_codeList.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), codeDropDownRect.width, 25), ""))
                            {
                                DisplayedCode = m_codeList[index];
                                m_showCodeScroll = false;
                                LastDisplayedCode = DisplayedCode;
                            }
                            GUI.Label(new Rect(5, (index * 25), codeDropDownRect.width, 25), "<b>" + m_codeList[index] + "</b>");
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
                            //DealWithCustomCheatCode(DisplayedCode);
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
                        scrollViewSelectVector = GUI.BeginScrollView(new Rect(selectDropDownRect.x, selectDropDownRect.y + 20, selectDropDownRect.width, selectDropDownRect.height), scrollViewSelectVector, new Rect(0, 0, selectDropDownRect.width - 20, Mathf.Max(selectDropDownRect.height, (list.Length * 25))));

                        GUI.Box(new Rect(0, 0, selectDropDownRect.width, Mathf.Max(selectDropDownRect.height, (list.Length * 25))), "");

                        for (int index = 0; index < list.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), selectDropDownRect.width, 25), ""))
                            {
                                m_selectShow = false;
                                m_selectIndexNumber = index;
                            }

                            GUI.Label(new Rect(selectDropDownRect.x, (index * 25), selectDropDownRect.width, 25), list[index]);
                        }
                        GUI.EndScrollView();
                    }
                    else
                    {
                        GUI.Label(new Rect(selectDropDownRect.x, selectDropDownRect.y, selectDropDownRect.width, 25), list[m_selectIndexNumber]);
                    }

                    // enter button

                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("<b>Enter</b>", GUILayout.Height(30f)))
                        {
                            //DealWithCustomCheatCode(DisplayedCode);

                        }
                    }
                    GUILayout.EndVertical();
                    #endregion

                    #region Button Debug
                    //
                    // Button Debug
                    //

                    GUILayout.Label("<b>Button</b>");

                    int lines = m_debugButtonMethodNum / m_numberOfButtonsPerLine + 1;
                    if(m_debugButtonMethodNum == 0)
                    {
                        lines = 0;
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
                                            //SendMessage(Dict[numPerLine * i + j]);
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