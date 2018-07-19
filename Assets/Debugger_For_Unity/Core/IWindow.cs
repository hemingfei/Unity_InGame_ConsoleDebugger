#region Author
/// <summary>--------------------------------------------------
///		Author:			He, Mingfei
///		Namespace:		<Debugger_For_Unity.something>
///		Class:			IWindow
///		Date:			7/18/2018 2:29:59 PM
/// </summary>--------------------------------------------------
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// The Window Interface
	/// </summary>
	public interface IWindow 
	{
        /// <summary>
        /// Init
        /// </summary>
        /// <param name="args"></param>
        void OnWindowAwake(params object[] args);

        /// <summary>
        /// Destroy
        /// </summary>
        void OnWindowDestroy();

        /// <summary>
        /// Enter
        /// </summary>
        void OnWindowEnter();

        /// <summary>
        /// Exit
        /// </summary>
        void OnWindowExit();

        /// <summary>
        /// Update, draw the window
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="unscaledDeltaTime"></param>
        void OnWindowStay(float deltaTime, float unscaledDeltaTime);

        /// <summary>
        /// GUI
        /// </summary>
        void OnWindowDraw();
    }
}