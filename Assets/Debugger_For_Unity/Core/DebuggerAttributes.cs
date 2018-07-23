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
    /// Button
    /// </summary>
    public class DebuggerButtonDebugAttribute : Attribute
    {
        public string Description { get; private set; }

        public int Priority { get; private set; }

        public DebuggerButtonDebugAttribute(string descrition, int priority = 0)
        {
            this.Description = descrition;
            this.Priority = priority;
        }
    }

    /// <summary>
    /// Select
    /// </summary>
    public class DebuggerSelectDebugAttribute : Attribute
    {
        public string Description { get; private set; }

        public int Priority { get; private set; }

        public DebuggerSelectDebugAttribute(string descrition, int priority = 0)
        {
            this.Description = descrition;
            this.Priority = priority;
        }
    }

    /// <summary>
    /// Code
    /// </summary>
    public class DebuggerCodeDebugAttribute : Attribute
    {
        public string CustomCode { get; set; }

        public string Description { get; private set; }

        public int Priority { get; private set; }

        public DebuggerCodeDebugAttribute(string customCode, string descrition, int priority = 0)
        {
            this.CustomCode = customCode;
            this.Description = descrition;
            this.Priority = priority;
        }
    }
}