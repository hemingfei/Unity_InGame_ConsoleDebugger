#region Author
/// <summary>--------------------------------------------------
///		Author:			He, Mingfei
///		Namespace:		<Debugger_For_Unity.something>
///		Class:			WindowGroup
///		Date:			7/18/2018 2:52:38 PM
/// </summary>--------------------------------------------------
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// Window Group
	/// </summary>
	public class WindowGroup : IWindow
    {
        #region  Attributes and Properties
        /// <summary>
        /// Private Members
        /// </summary>
        private readonly List<KeyValuePair<string, IWindow>> m_windows;
        private string[] m_windowNames;

        /// <summary>
        /// Properties
        /// </summary>

        public int WindowCount
        {
            get
            {
                return m_windows.Count;
            }
        }

        public int SelectedIndex { get; set; }

        public IWindow SelectedWindow
        {
            get
            {
                if (SelectedIndex >= m_windows.Count)
                {
                    return null;
                }

                return m_windows[SelectedIndex].Value;
            }
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public WindowGroup()
        {
            m_windows = new List<KeyValuePair<string, IWindow>>();
            SelectedIndex = 0;
            m_windowNames = null;
        }

        /// <summary>
        /// Register the Window or Window Group
        /// </summary>
        /// <param name="path"></param>
        /// <param name="window"></param>
        public void RegisterWindow(string path, IWindow window)
        {
            int pos = path.IndexOf('/');
            if (pos < 0 || pos >= path.Length - 1)
            {
                if (GetSpecificWindow(path) != null)
                {
                    Debug.LogWarning(path + " Window Already Registered");
                    return;
                }

                m_windows.Add(new KeyValuePair<string, IWindow>(path, window));
                RefreshWindowNames();
            }
            else
            {
                string windowGroupName = path.Substring(0, pos);
                string leftPath = path.Substring(pos + 1);
                WindowGroup windowGroup = (WindowGroup)GetSpecificWindow(windowGroupName);
                if (windowGroup == null)
                {
                    if (GetSpecificWindow(windowGroupName) != null)
                    {
                        Debug.LogWarning(path + " Window Group Already Registered");
                    }
                    windowGroup = new WindowGroup();
                    m_windows.Add(new KeyValuePair<string, IWindow>(windowGroupName, windowGroup));
                    RefreshWindowNames();
                }
                windowGroup.RegisterWindow(leftPath, window);
            }
        }

        /// <summary>
        /// Get all the names of registered windows
        /// </summary>
        public string[] GetWindowNames()
        {
            return m_windowNames;
        }

        /// <summary>
        /// Get the window or windows groups
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IWindow GetWindow(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            int pos = path.IndexOf('/');
            if (pos < 0 || pos >= path.Length - 1)
            {
                return GetSpecificWindow(path); // get the window
            }

            string windowGroupName = path.Substring(0, pos);
            string leftPath = path.Substring(pos + 1);
            WindowGroup WindowGroup = (WindowGroup)GetSpecificWindow(windowGroupName); // get the window group
            if (WindowGroup == null)
            {
                return null;
            }

            return WindowGroup.GetWindow(leftPath);
        }

        /// <summary>
        /// Select the window or window groups
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SelectWindow(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            int pos = path.IndexOf('/');
            if (pos < 0 || pos >= path.Length - 1)
            {
                return SelectSpecificWindow(path);
            }

            string windowGroupName = path.Substring(0, pos);
            string leftPath = path.Substring(pos + 1);
            WindowGroup windowGroup = (WindowGroup)GetSpecificWindow(windowGroupName);
            if (windowGroup == null || !SelectSpecificWindow(windowGroupName))
            {
                return false;
            }

            return windowGroup.SelectWindow(leftPath);
        }
        #endregion

        #region Interface Public Methods
        public void OnWindowAwake(params object[] args)
        {
            
        }

        public void OnWindowDestroy()
        {
            foreach (KeyValuePair<string, IWindow> debuggerWindow in m_windows)
            {
                debuggerWindow.Value.OnWindowDestroy();
            }

            m_windows.Clear();
        }

        public void OnWindowEnter()
        {
            SelectedWindow.OnWindowEnter();
        }

        public void OnWindowExit()
        {
            SelectedWindow.OnWindowExit();
        }

        public void OnWindowStay(float deltaTime, float unscaledDeltaTime)
        {
            SelectedWindow.OnWindowStay(deltaTime, unscaledDeltaTime);
        }

        public void OnWindowDraw()
        {
            
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Add the window name into the array after the window registered in
        /// </summary>
        private void RefreshWindowNames()
        {
            m_windowNames = new string[m_windows.Count];
            int index = 0;
            foreach (KeyValuePair<string, IWindow> debuggerWindow in m_windows)
            {
                m_windowNames[index++] = debuggerWindow.Key;
            }
        }

        /// <summary>
        /// get the window
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IWindow GetSpecificWindow(string name)
        {
            foreach (KeyValuePair<string, IWindow> window in m_windows)
            {
                if (window.Key == name)
                {
                    return window.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// select the window
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool SelectSpecificWindow(string name)
        {
            for (int i = 0; i < m_windows.Count; i++)
            {
                if (m_windows[i].Key == name)
                {
                    SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}