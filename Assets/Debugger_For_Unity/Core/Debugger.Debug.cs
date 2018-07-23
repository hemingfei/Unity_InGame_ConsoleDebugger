#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Debug
//		Date:			7/20/2018 10:00:36 AM
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
    /// Partial Class
    /// </summary>
    public partial class Debugger
    {
        public partial class Debug
        {
            #region  Attributes and Properties
            public Code CodeWindow = new Code();

            public Select SelectWindow = new Select();

            public Button ButtonWindow = new Button();

            public Debugger Debugger { get; set; }

            #endregion

            public Debug()
            {
                CodeWindow.Debug = this;
                SelectWindow.Debug = this;
                ButtonWindow.Debug = this;
            }

        }
    }
}