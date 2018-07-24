#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Console.Logmsg
//		Date:			7/19/2018 11:05:22 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using UnityEngine;

namespace Debugger_For_Unity
{
    public partial class Debugger
    {
        /// <summary>
        /// partial class, log message information
        /// </summary>
        private sealed partial class Console
        {
            /// <summary>
            /// Log Message
            /// </summary>
            private sealed class LogMsg
            {
                #region Public Methods
                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="logType"></param>
                /// <param name="logMessage"></param>
                /// <param name="stackTrack"></param>
                public LogMsg(LogType logType, string logMessage, string stackTrack)
                {
                    LogTime = DateTime.Now;
                    LogType = logType;
                    LogMessage = logMessage;
                    StackTrack = stackTrack;
                }
                #endregion

                #region  Attributes and Properties
                /// <summary>
                /// Properties
                /// </summary>

                public DateTime LogTime { get; private set; }

                public LogType LogType { get; private set; }

                public string LogMessage { get; private set; }

                public string StackTrack { get; private set; }
                #endregion
            }
        }
    }
}