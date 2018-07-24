#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug.DebugAttribute
//		Date:			7/20/2018 10:16:08 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

    /// <summary>
    /// Button Attribute
    /// </summary>
    public class DebuggerButtonDebugAttribute : Attribute
    {
        #region  Attributes and Properties
        public string Description { get; private set; }

        public int Priority { get; private set; }
        #endregion

        #region Public Methods
        public DebuggerButtonDebugAttribute(string description, int priority = 0)
        {
            this.Description = description;
            this.Priority = priority;
        }
        #endregion
    }

    /// <summary>
    /// Select Attribute
    /// </summary>
    public class DebuggerSelectDebugAttribute : Attribute
    {
        #region  Attributes and Properties
        public string Description { get; private set; }

        public int Priority { get; private set; }
        #endregion

        #region Public Methods
        public DebuggerSelectDebugAttribute(string description, int priority = 0)
        {
            this.Description = description;
            this.Priority = priority;
        }
        #endregion
    }

    /// <summary>
    /// Code Attribute
    /// </summary>
    public class DebuggerCodeDebugAttribute : Attribute
    {
        #region  Attributes and Properties
        public string CustomCode { get; set; }

        public string Description { get; private set; }

        public int Priority { get; private set; }
        #endregion

        #region Public Methods
        public DebuggerCodeDebugAttribute(string customCode, string description, int priority = 0)
        {
            this.CustomCode = customCode;
            this.Description = description;
            this.Priority = priority;
        }
        #endregion
    }
}