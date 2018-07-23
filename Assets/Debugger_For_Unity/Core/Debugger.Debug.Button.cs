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
    /// Class Explanation
    /// </summary>
    public partial class Debugger
    {

        [Serializable]
        public class Button : IWindow
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

            // button debug
            [SerializeField]
            public int m_numberOfButtonsPerLine = 5;
            private int m_debugButtonMethodNum = 0;
            private Dictionary<int, string> m_debugButtonMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugButtonDescritionDict = new Dictionary<int, string>();

            #endregion

            public Button(Debug debug)
            {
                this.Debug = debug;
            }
            public Button()
            {

            }

            #region Private Methods
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
                    }
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