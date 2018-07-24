#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug.Select
//		Date:			7/23/2018 9:17:13 AM
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
        public class Select : IWindow
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

            // select debug
            [SerializeField]
            private GUIStyle m_backGroundStyle = null;
            private int m_debugSelectMethodNum = 0;
            private Dictionary<int, string> m_debugSelectMethodDict = new Dictionary<int, string>();
            private Dictionary<int, string> m_debugSelectDescritionDict = new Dictionary<int, string>();
            private string[] m_debugSelectDescriptionArray;
            private int m_selectIndexNumber;
            private bool m_selectShow = false;
            private Vector2 scrollViewSelectVector = Vector2.zero;

            #endregion


            #region Private Methods
            private void DealWithSelect(int index)
            {
                MethodInfo method = typeof(Debugger).GetMethod(m_debugSelectMethodDict[index]);
                if (method.GetParameters().Length > 0)
                {
                    UnityEngine.Debug.LogWarning(method.Name + " Method has unwanted parameters, should not be registered as Select, RETURN.");
                    return;
                }
                method.Invoke(Debugger, null);
            }
            #endregion

            public Select(Debug debug)
            {
                this.Debug = debug;
            }

            public Select()
            {

            }

            #region Interface Public Methods
            public void OnWindowAwake(params object[] args)
            {
                Type type = typeof(Debugger);

                Dictionary<int, int> sortPriorityDict = new Dictionary<int, int>();

                foreach (MethodInfo method in type.GetMethods())
                {
                    foreach (Attribute attr in method.GetCustomAttributes(true))
                    {
                        if (attr is DebuggerSelectDebugAttribute)
                        {
                            m_debugSelectMethodDict.Add(m_debugSelectMethodNum, method.Name);
                            m_debugSelectDescritionDict.Add(m_debugSelectMethodNum, ((DebuggerSelectDebugAttribute)attr).Description);
                            sortPriorityDict.Add(m_debugSelectMethodNum, ((DebuggerSelectDebugAttribute)attr).Priority);
                            m_debugSelectMethodNum++;
                        }
                    }
                }

                //
                // Priority
                //

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
                    m_newMethodDict.Add(index, m_debugSelectMethodDict[pair.Key]);
                    m_newDescritionDict.Add(index, m_debugSelectDescritionDict[pair.Key]);
                    index++;
                }

                m_debugSelectMethodDict = m_newMethodDict;
                m_debugSelectDescritionDict = m_newDescritionDict;


                m_debugSelectDescriptionArray = m_debugSelectDescritionDict.Values.ToArray();
            }


            public void OnWindowDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {

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

                    Rect selectDropDownRect = new Rect(5, 30, 400, 100);
                    if (m_selectShow)
                    {
                        scrollViewSelectVector = GUI.BeginScrollView(new Rect(selectDropDownRect.x, selectDropDownRect.y + 60, selectDropDownRect.width, selectDropDownRect.height), scrollViewSelectVector, new Rect(0, 0, selectDropDownRect.width - 20, Mathf.Max(selectDropDownRect.height, (m_debugSelectDescriptionArray.Length * 25))));

                        GUI.Box(new Rect(0, 0, selectDropDownRect.width, Mathf.Max(selectDropDownRect.height, (m_debugSelectDescriptionArray.Length * 25))), "");

                        for (int index = 0; index < m_debugSelectDescriptionArray.Length; index++)
                        {
                            if (GUI.Button(new Rect(0, (index * 25), selectDropDownRect.width, 25), ""))
                            {
                                m_selectShow = false;
                                m_selectIndexNumber = index;
                            }

                            GUI.Label(new Rect(selectDropDownRect.x, (index * 25), selectDropDownRect.width, 25), String.Format("<b>{0}</b>", m_debugSelectDescriptionArray[index]), m_backGroundStyle);
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