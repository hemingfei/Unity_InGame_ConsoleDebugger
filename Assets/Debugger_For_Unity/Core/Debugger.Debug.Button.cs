#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug.Button
//		Date:			7/23/2018 9:17:21 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Debugger_For_Unity
{

    /// <summary>
    /// Partial clsss, button window
    /// </summary>
    public partial class Debugger
    {
        [Serializable]
        public class Button : IWindow
        {
            #region  Attributes and Properties
            public Debug Debug { get; set; }
            public Debugger Debugger { get; set; }
            /// <summary>
            /// Private Members
            /// </summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            // button debug
            [SerializeField]
            public int m_numberOfButtonsPerLine = 5;
            private int m_debugButtonMethodNum = 0;
            private Dictionary<int, string> m_debugButtonMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugButtonDescritionDict = new Dictionary<int, string>();

            #endregion

            #region Public Methods
            /// <summary>
            /// constructors
            /// </summary>
            /// <param name="debug"></param>
            public Button(Debug debug)
            {
                this.Debug = debug;
            }
            public Button()
            {

            }
            #endregion

            #region Private Methods
            /// <summary>
            /// invode custom methods
            /// </summary>
            /// <param name="index"></param>
            private void DealWithButton(int index)
            {
                MethodInfo method = typeof(Debugger).GetMethod(m_debugButtonMethodDict[index]);
                if (method.GetParameters().Length > 0)
                {
                    UnityEngine.Debug.LogWarning(method.Name + " Method has unwanted parameters, should not be registered as button, RETURN.");
                    return;
                }
                method.Invoke(Debugger, null);
            }
            #endregion

            #region Interface Public Methods
            public void OnWindowAwake(params object[] args)
            {
                Type type = typeof(Debugger);

                Dictionary<int, int> sortPriorityDict = new Dictionary<int, int>();

                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (Attribute attr in method.GetCustomAttributes(true))
                    {
                        if (attr is DebuggerButtonDebugAttribute)
                        {
                            m_debugButtonMethodDict.Add(m_debugButtonMethodNum, method.Name);
                            m_debugButtonDescritionDict.Add(m_debugButtonMethodNum, ((DebuggerButtonDebugAttribute)attr).Description);
                            sortPriorityDict.Add(m_debugButtonMethodNum, ((DebuggerButtonDebugAttribute)attr).Priority);
                            m_debugButtonMethodNum++;
                        }
                    }
                }

                // Priority

                // ... Use LINQ to specify sorting by value.
                var items = from pair in sortPriorityDict
                            orderby pair.Value ascending
                            select pair;

                Dictionary<int, string> m_newMethodDict = new Dictionary<int, string>();
                Dictionary<int, string> m_newDescritionDict = new Dictionary<int, string>();
                int index = 0;
                // re value
                foreach (KeyValuePair<int, int> pair in items)
                {
                    m_newMethodDict.Add(index, m_debugButtonMethodDict[pair.Key]);
                    m_newDescritionDict.Add(index, m_debugButtonDescritionDict[pair.Key]);
                    index++;
                }

                m_debugButtonMethodDict = m_newMethodDict;
                m_debugButtonDescritionDict = m_newDescritionDict;

                // just in cast the SerializeField parameter set wrong in awake
                if (m_numberOfButtonsPerLine <= 0)
                {
                    m_numberOfButtonsPerLine = 1;
                }
            }

            public void OnWindowDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {

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