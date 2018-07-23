#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug.CustomMethods
//		Date:			7/20/2018 10:10:08 AM
/// </summary>--------------------------------------------------
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// Class Explanation
	/// </summary>
	public partial class Debugger
	{

        #region Public Methods
        [DebuggerButtonDebug("test1", 1)]
        public void Test1()
        {
            UnityEngine.Debug.Log("TEST1 LOG");
        }


        [DebuggerButtonDebug("test2", 1)]
        [DebuggerSelectDebug("test2 select", 1)]
        [DebuggerCodeDebug("test2 code", "test2 description")]
        public void Test2(string a, string bb)
        {
            UnityEngine.Debug.Log("TEST2 LOG");
        }

        [DebuggerButtonDebug("test3", 1)]
        public void Test3()
        {
            UnityEngine.Debug.Log("TEST3 LOG");
        }

        [DebuggerCodeDebug("test4 code", "test4 description")]
        public void Test4()
        {
            UnityEngine.Debug.Log("TEST4 LOG");
        }
        [DebuggerButtonDebug("test5", 1)]
        [DebuggerSelectDebug("test5 select", 1)]
        [DebuggerCodeDebug("test5 code", "test5 description")]
        public void Test5()
        {
            UnityEngine.Debug.Log("TEST5 LOG");
        }
        #endregion


        #region Static Methods

        #endregion
    }
}