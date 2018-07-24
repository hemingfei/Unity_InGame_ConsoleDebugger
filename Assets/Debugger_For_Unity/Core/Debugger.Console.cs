#region Author
/// <summary>--------------------------------------------------
// 		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Console
//		Date:			7/18/2018 10:59:57 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

    /// <summary>
    /// Partial Class
    /// </summary>
    public partial class Debugger
	{
        [Serializable]
        private sealed partial class Console : IWindow
        {
            #region  Attributes and Properties

            /// <summary>
            /// Properties
            /// </summary>
            public int InfoCount { get; private set; }

            public int WarningCount { get; private set; }

            public int ErrorCount { get; private set; }

            /// <summary>
            /// Private Members
            /// </summary>

            [SerializeField]
            private int m_maxLines = 100;

            private Vector2 m_logScrollPosition = Vector2.zero;

            private Vector2 m_stackScrollPosition = Vector2.zero;

            private bool m_lockScroll = true;

            private bool m_collapse = false;

            private bool m_infoFilter = true;

            private bool m_warningFilter = true;

            private bool m_errorFilter = true;

            private LinkedList<LogMsg> m_logs = new LinkedList<LogMsg>();

            private LinkedListNode<LogMsg> m_selectedLog = null;

            private Dictionary<string, int> m_collapseCheckLogs = new Dictionary<string, int>();

            private Dictionary<string, LogMsg> m_collapseLogs = new Dictionary<string, LogMsg>();
            #endregion

            #region Interface Public Methods
            public void OnWindowAwake(params object[] args)
            {
                Application.logMessageReceived += OnLogMessageReceived;

                // just in cast the SerializeField parameter set wrong in awake
                if (m_maxLines < 0)
                {
                    m_maxLines = 0;
                }
            }

            public void OnWindowDestroy()
            {
                Application.logMessageReceived -= OnLogMessageReceived;
                m_logs.Clear();
            }

            public void OnWindowDraw()
            {
                // update the count of different types of logs
                RefreshCount();

                // update collapse logs count
                RefreshCollapseCount();

                // draw the clear button and show the toggles of lock scroll and three types of log filters
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Clear", GUILayout.Width(100f)))
                    {
                        m_logs.Clear();
                    }
                    m_collapse = GUILayout.Toggle(m_collapse, "Collapse", GUILayout.Width(120f));
                    m_infoFilter = GUILayout.Toggle(m_infoFilter, string.Format("Info ({0}) \t", InfoCount.ToString()), GUILayout.Width(80f));
                    m_warningFilter = GUILayout.Toggle(m_warningFilter, string.Format("Warning ({0}) \t", WarningCount.ToString()), GUILayout.Width(100f));
                    m_errorFilter = GUILayout.Toggle(m_errorFilter, string.Format("Error ({0}) \t", ErrorCount.ToString()), GUILayout.Width(100f));
                    GUILayout.FlexibleSpace();
                    m_lockScroll = GUILayout.Toggle(m_lockScroll, "Lock Scroll", GUILayout.Width(100f));
                }
                GUILayout.EndHorizontal();

                // draw the log message console panel
                GUILayout.BeginVertical("box");
                {
                    if (m_lockScroll)
                    {
                        m_logScrollPosition.y = float.MaxValue;
                    }

                    m_logScrollPosition = GUILayout.BeginScrollView(m_logScrollPosition, GUILayout.Height(200f));
                    {
                        bool selected = false;
                        for (LinkedListNode<LogMsg> i = m_logs.First; i != null; i = i.Next)
                        {
                            switch (i.Value.LogType)
                            {
                                case LogType.Log:
                                    if (!m_infoFilter)
                                    {
                                        continue;
                                    }
                                    break;
                                case LogType.Warning:
                                    if (!m_warningFilter)
                                    {
                                        continue;
                                    }
                                    break;
                                case LogType.Error:
                                    if (!m_errorFilter)
                                    {
                                        continue;
                                    }
                                    break;
                            }

                            if (m_collapse)
                            {
                                LogType type = i.Value.LogType;
                                string msg = i.Value.LogMessage;
                                string stack = i.Value.StackTrack;
                                string keyCheck = type.ToString() + msg + stack;
                                if (m_collapseLogs[keyCheck] == i.Value)
                                {
                                    string s = string.Format("[{0}] {1} ", m_collapseCheckLogs[keyCheck], GetLogMsgString(i.Value));
                                    if (GUILayout.Toggle(m_selectedLog == i, s))
                                    {
                                        selected = true;
                                        if (m_selectedLog != i)
                                        {
                                            m_selectedLog = i;
                                            m_stackScrollPosition = Vector2.zero;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // not collapse
                                if (GUILayout.Toggle(m_selectedLog == i, GetLogMsgString(i.Value)))
                                {
                                    selected = true;
                                    if (m_selectedLog != i)
                                    {
                                        m_selectedLog = i;
                                        m_stackScrollPosition = Vector2.zero;
                                    }
                                }
                            }
                        }
                        if (!selected)
                        {
                            m_selectedLog = null;
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                // draw the log stack console panel
                GUILayout.BeginVertical("box");
                {
                    m_stackScrollPosition = GUILayout.BeginScrollView(m_stackScrollPosition, GUILayout.Height(100f));
                    {
                        if (m_selectedLog != null)
                        {
                            GUILayout.BeginHorizontal();
                            Color32 color = GetLogMsgColor(m_selectedLog.Value.LogType);
                            GUILayout.Label(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_selectedLog.Value.LogMessage));
                            if (GUILayout.Button("COPY", GUILayout.Width(60f), GUILayout.Height(30f)))
                            {
                                TextEditor textEditor = new TextEditor();
                                textEditor.text = string.Format("{0}\n\n{1}", m_selectedLog.Value.LogMessage, m_selectedLog.Value.StackTrack);
                                textEditor.OnFocus();
                                textEditor.Copy();
                            }
                            GUILayout.EndHorizontal();
                            GUILayout.Label(m_selectedLog.Value.StackTrack);
                        }
                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndVertical();
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

            #region Public Methods
            /// <summary>
            /// refresh the log msg specific type count
            /// </summary>
            public void RefreshCount()
            {
                InfoCount = 0;
                WarningCount = 0;
                ErrorCount = 0;
                for (LinkedListNode<LogMsg> i = m_logs.First; i != null; i = i.Next)
                {
                    switch (i.Value.LogType)
                    {
                        case LogType.Log:
                            InfoCount++;
                            break;
                        case LogType.Warning:
                            WarningCount++;
                            break;
                        case LogType.Error:
                            ErrorCount++;
                            break;
                    }
                }
            }
            #endregion

            #region Private Methods
            /// <summary>
            /// += unity's log message event
            /// </summary>
            /// <param name="logMessage"></param>
            /// <param name="stackTrace"></param>
            /// <param name="logType"></param>
            private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
            {
                if (logType == LogType.Assert || logType == LogType.Exception)
                {
                    logType = LogType.Error;
                }

                m_logs.AddLast(new LogMsg(logType, logMessage, stackTrace));
                while (m_logs.Count > m_maxLines)
                {
                    m_logs.RemoveFirst();
                }
            }

            /// <summary>
            /// get the message from LogMsg
            /// </summary>
            /// <param name="logNode"></param>
            /// <returns></returns>
            private string GetLogMsgString(LogMsg logNode)
            {
                Color32 color = GetLogMsgColor(logNode.LogType);
                return string.Format("<color=#{0}{1}{2}{3}>{4}{5}</color>",
                    color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"),
                    logNode.LogTime.ToString("[HH:mm:ss.fff]"), logNode.LogMessage);
            }

            /// <summary>
            /// get the color from the type
            /// </summary>
            /// <param name="logType"></param>
            /// <returns></returns>
            private Color32 GetLogMsgColor(LogType logType)
            {
                Color32 color = Color.white;
                switch (logType)
                {
                    case LogType.Log:
                        color = Color.white;
                        break;
                    case LogType.Warning:
                        color = Color.yellow;
                        break;
                    case LogType.Error:
                        color = Color.red;
                        break;
                    default:
                        color = Color.red;
                        break;
                }

                return color;
            }

            /// <summary>
            /// calculate the collapse logs count
            /// </summary>
            private  void RefreshCollapseCount()
            {
                m_collapseCheckLogs.Clear();
                m_collapseLogs.Clear();
                for (LinkedListNode<LogMsg> i = m_logs.First; i != null; i = i.Next)
                {
                    LogType type = i.Value.LogType;
                    string msg = i.Value.LogMessage;
                    string stack = i.Value.StackTrack;
                    string keyCheck = type.ToString() + msg + stack;
                    if (m_collapseCheckLogs.ContainsKey(keyCheck))
                    {
                        m_collapseCheckLogs[keyCheck]++;
                        m_collapseLogs[keyCheck] = i.Value;
                    }
                    else
                    {
                        m_collapseCheckLogs.Add(keyCheck, 1);
                        m_collapseLogs.Add(keyCheck, i.Value);
                    }
                }
            }
            #endregion
        }  
    }
}