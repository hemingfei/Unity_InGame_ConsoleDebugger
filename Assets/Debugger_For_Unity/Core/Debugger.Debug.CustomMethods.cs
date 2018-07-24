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
	/// Partial Class Of Debugger (MonoBehaviour)
    /// Custom Methods Written Here
    /// Remenber add the attributes
	/// </summary>
	public partial class Debugger
	{
        #region Engion Methods
        /// <summary>
        /// In case you don't know how to use the Unity Methdos In Partial Class
        /// </summary>
        private void UnityStartForPartialCustomMethods()
        {

        }
        #endregion


        #region Public Methods
        [DebuggerButtonDebug("test1", -1)]
        public void Test1()
        {
            UnityEngine.Debug.Log("TEST1 LOG");
        }


        [DebuggerButtonDebug("test2", 1)]
        [DebuggerSelectDebug("test2 select", 1)]
        [DebuggerCodeDebug("test2   code", "test2 description", 2)]
        public void Test2(int a, string bb, int ccc, float ddd, bool eee)
        {
            int q = a + ccc;
            UnityEngine.Debug.Log("TEST2, 1st param " + a);
            UnityEngine.Debug.Log("TEST2, 2nd param " + bb);
            UnityEngine.Debug.Log("TEST2, 3rt param " + ccc);
            UnityEngine.Debug.Log("TEST2, 4th param " + ddd);
            UnityEngine.Debug.Log("TEST2, 5th param " + eee);
            UnityEngine.Debug.Log("TEST2, 1st param + 3rd param is : " + q);
        }

        [DebuggerButtonDebug("test3", 6)]
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
        [DebuggerSelectDebug("test5 select", 3)]
        [DebuggerCodeDebug("test5  code", "test5 description", 1)]
        public void Test5()
        {
            UnityEngine.Debug.Log("TEST5 LOG");
        }

        [DebuggerButtonDebug("test6", 0)]
        [DebuggerSelectDebug("test6 select", 1)]
        [DebuggerCodeDebug("test6  code", "test6 description")]
        public void Test6()
        {
            UnityEngine.Debug.Log("TEST6 LOG");
        }

        [DebuggerButtonDebug("test7", 0)]
        [DebuggerSelectDebug("test7 select", 1)]
        [DebuggerCodeDebug("test7  code", "test7 description")]
        public void Test7()
        {
            UnityEngine.Debug.Log("TEST7 LOG");
        }

        [DebuggerButtonDebug("test8", 0)]
        [DebuggerSelectDebug("test8 select", 0)]
        [DebuggerCodeDebug("test8  code", "test8 description")]
        public void Test8()
        {
            UnityEngine.Debug.Log("TEST8 LOG");
        }

        [DebuggerButtonDebug("test9")]
        [DebuggerSelectDebug("test9 select", 1)]
        [DebuggerCodeDebug("test9  code", "test9 description")]
        public void Test9()
        {
            UnityEngine.Debug.Log("TEST9 LOG");
        }

        [DebuggerButtonDebug("test10")]
        [DebuggerSelectDebug("test10 select", 1)]
        [DebuggerCodeDebug("test10  code", "test10 description")]
        public void Test10()
        {
            UnityEngine.Debug.Log("TEST10 LOG");
        }

        [DebuggerButtonDebug("test11", 1)]
        [DebuggerSelectDebug("test11 select", 1)]
        [DebuggerCodeDebug("test11  code", "test11 description")]
        public void Test11()
        {
            UnityEngine.Debug.Log("TEST11 LOG");
        }

        [DebuggerButtonDebug("test12", 1)]
        [DebuggerSelectDebug("test12 select", 1)]
        [DebuggerCodeDebug("test12  code", "test12 description")]
        public void Test12()
        {
            UnityEngine.Debug.Log("TEST12 LOG");
        }
        #endregion

    }
}