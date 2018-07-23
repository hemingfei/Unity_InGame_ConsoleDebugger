#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug.Code
//		Date:			7/23/2018 9:16:58 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Debugger_For_Unity
{

    /// <summary>
    /// Class Explanation
    /// </summary>
    public partial class Debugger
    {

        [Serializable]
        public class Code : IWindow
        {
            #region  Attributes and Properties
            //public Debugger Debugger { get; set; }
            public Debug Debug { get; set; }
            public Debugger Debugger { get; set; }
            /// <summary>
            /// Private Members
            /// </summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            // code debug
            [SerializeField]
            private GUIStyle m_codeBackGroundStyle = null;
            [SerializeField]
            private GUIStyle m_descriptionBackGroundStyle = null;
            [SerializeField]
            private GUIStyle m_inputAnswerStyle = null;
            private int m_debugCodeMethodNum = 0;
            private Dictionary<int, string> m_debugCodeMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeCustomCodeDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugCodeDescritionDict = new Dictionary<int, string>();
            private string[] m_debugCodeDescriptionArray;
            private string[] m_debugCodeCustomCodeArray;
            private string[] m_matchedCodeArray = { };
            private string[] m_matchedCodeDescription = { };
            private bool m_showCodeScroll = false;
            private Vector2 scrollViewCodeVector = Vector2.zero;
            private string m_displayedCode = "";
            private string m_lastDisplayedCode = "";
            private List<string> m_typedCodeHistory = new List<string>(20);
            private int m_typedCodeIndex = 0;
            private string m_inputAnswer = "";
            #endregion

            public Code(Debug debug)
            {
                this.Debug = debug;
            }
            public Code()
            {

            }

            #region Private Methods
            /// <summary>
            /// refresh the matched input code to give tips
            /// </summary>
            private void RefreshCodeList()
            {
                Dictionary<int, string> refreshCode = new Dictionary<int, string>();
                Dictionary<int, string> refreshDescription = new Dictionary<int, string>();
                int index = 0;

                for (int i = 0; i < m_debugCodeCustomCodeArray.Length; i++)
                {
                    if (m_debugCodeCustomCodeArray[i].StartsWith(m_displayedCode, System.StringComparison.CurrentCultureIgnoreCase) || m_debugCodeCustomCodeArray[i].Contains(m_displayedCode))
                    {
                        refreshCode[index] = m_debugCodeCustomCodeArray[i];
                        refreshDescription[index] = m_debugCodeDescriptionArray[i];
                        index++;
                    }
                }
                m_matchedCodeArray = refreshCode.Values.ToArray();
                m_matchedCodeDescription = refreshDescription.Values.ToArray();
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
                    m_inputAnswer = "No Such Code";
                    return;
                }
                int key = m_debugCodeCustomCodeDict.FirstOrDefault(x => x.Value == ss[0]).Key;
                MethodInfo method = typeof(Debugger).GetMethod(m_debugCodeMethodDict[key]);
                if (codeSplitParamNum != (method.GetParameters().Length + 1))
                {
                    UnityEngine.Debug.Log("Wrong Code, Reason: Number of Parameters");
                    m_inputAnswer = "Wrong Code, Reason: Number of Parameters";
                    return;
                }
                string[] param = new string[codeSplitParamNum - 1];
                for (int i = 0; i < (codeSplitParamNum - 1); i++)
                {
                    param[i] = ss[i + 1];
                }
                UnityEngine.Debug.Log("Apply Code: [" + code + "] Successful.");
                m_inputAnswer = "Apply Code: [" + code + "] Successful.";
                method.Invoke(Debugger, param);
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
                        if (attr is DebuggerCodeDebugAttribute)
                        {
                            m_debugCodeMethodDict.Add(m_debugCodeMethodNum, method.Name);

                            string code = ((DebuggerCodeDebugAttribute)attr).CustomCode;
                            code = new Regex("[\\s]+").Replace(code, "_");
                            m_debugCodeCustomCodeDict.Add(m_debugCodeMethodNum, code);

                            string des = ((DebuggerCodeDebugAttribute)attr).Description;
                            //des = new Regex("[\\s]+").Replace(des, "_");
                            m_debugCodeDescritionDict.Add(m_debugCodeMethodNum, des);
                            m_debugCodeMethodNum++;
                        }
                    }
                }

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
                    m_displayedCode = GUILayout.TextField(m_displayedCode, 100, GUILayout.Height(25f));
                    //}
                    //GUILayout.EndVertical();

                    if (m_lastDisplayedCode != m_displayedCode)
                    {
                        m_showCodeScroll = true;
                        m_lastDisplayedCode = m_displayedCode;
                        RefreshCodeList();
                    }


                    // dropdown tips

                    if (m_displayedCode != "" && m_showCodeScroll != false)
                    {
                        Rect codeDropDownRect = new Rect(0, 0, 400, 20);

                        scrollViewCodeVector = GUI.BeginScrollView(new Rect(5, 100, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))), scrollViewCodeVector, new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))));

                        GUI.Box(new Rect(0, 0, codeDropDownRect.width, Mathf.Max(codeDropDownRect.height, (m_matchedCodeArray.Length * 25))), "");

                        for (int index = 0; index < m_matchedCodeArray.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), codeDropDownRect.width, 25), ""))
                            {
                                m_displayedCode = String.Format("{0}", m_matchedCodeArray[index]);
                                m_showCodeScroll = false;
                                m_lastDisplayedCode = m_displayedCode;
                            }
                            GUI.Label(new Rect(5, (index * 25), codeDropDownRect.width / 2, 25), String.Format("<b>{0}</b>", m_matchedCodeArray[index]), m_codeBackGroundStyle);
                            GUI.Label(new Rect(5 + codeDropDownRect.width / 2, (index * 25), codeDropDownRect.width / 2, 25), String.Format("<b>{0}</b>", m_matchedCodeDescription[index]), m_descriptionBackGroundStyle);
                        }
                        GUI.EndScrollView();
                    }

                    // clear and enter button

                    //GUILayout.BeginVertical("box");
                    //{
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("<b><-</b>", GUILayout.Width(50f), GUILayout.Height(30f)))
                        {
                            if (m_typedCodeIndex > 0 && m_typedCodeIndex <= m_typedCodeHistory.Count)
                            {
                                m_displayedCode = m_typedCodeHistory[m_typedCodeIndex - 1];
                                m_typedCodeIndex--;
                            }
                            else
                            {
                                m_typedCodeIndex = m_typedCodeHistory.Count;
                            }
                        }
                        if (GUILayout.Button("<b>-></b>", GUILayout.Width(50f), GUILayout.Height(30f)))
                        {
                            if (m_typedCodeIndex > 0 && m_typedCodeIndex <= m_typedCodeHistory.Count)
                            {
                                m_displayedCode = m_typedCodeHistory[m_typedCodeIndex - 1];
                                m_typedCodeIndex++;
                            }
                            else
                            {
                                m_typedCodeIndex = m_typedCodeHistory.Count;
                            }
                        }

                        if (GUILayout.Button("<b>Clear</b>", GUILayout.Width(100f), GUILayout.Height(30f)))
                        {
                            m_displayedCode = "";
                            m_typedCodeIndex = m_typedCodeHistory.Count;
                            m_inputAnswer = "";
                        }

                        if (GUILayout.Button("<b>Enter</b>", GUILayout.Height(30f)))
                        {
                            DealWithCustomCode(m_displayedCode);
                            TextEditor textEditor = new TextEditor();
                            textEditor.text = m_displayedCode;
                            textEditor.OnFocus();
                            textEditor.Copy();
                            m_typedCodeHistory.Add(m_displayedCode);
                            m_typedCodeIndex = m_typedCodeHistory.Count;
                            m_displayedCode = "";
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUI.Label(new Rect(220, 10, 100, 25), String.Format("{0}", m_inputAnswer), m_inputAnswerStyle);

                    //}
                    //GUILayout.EndVertical();
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