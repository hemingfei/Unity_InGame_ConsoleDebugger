#region Author
/// <summary>--------------------------------------------------
///		Author:			He, Mingfei
///		Namespace:		<Debugger_For_Unity.something>
///		Class:			DebuggerManager
///		Date:			7/18/2018 4:33:29 PM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// Manager which contains the window root that the windows registed in
	/// </summary>
	public class DebuggerManager
	{
        #region  Attributes and Properties
        /// <summary>
        /// Properties
        /// </summary>
        public WindowGroup DebuggerWindowRoot
        {
            get
            {
                return m_debuggerWindowRoot;
            }
        }

        /// <summary>
        /// Private Members
        /// </summary>
        private readonly WindowGroup m_debuggerWindowRoot;
        #endregion

        #region Public Methods
        public DebuggerManager()
        {
            m_debuggerWindowRoot = new WindowGroup();
        }

        public void RegisterDebuggerWindow(string path, IWindow debuggerWindow, params object[] args)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Path is invalid.");
            }

            if (debuggerWindow == null)
            {
                throw new Exception("Debugger window is invalid.");
            }

            m_debuggerWindowRoot.RegisterWindow(path, debuggerWindow);
            debuggerWindow.OnWindowAwake(args);
        }

        public IWindow GetDebuggerWindow(string path)
        {
            return m_debuggerWindowRoot.GetWindow(path);
        }

        public bool SelectDebuggerWindow(string path)
        {
            return m_debuggerWindowRoot.SelectWindow(path);
        }
        #endregion
    }
}