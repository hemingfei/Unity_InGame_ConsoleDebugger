#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Billboard
//		Date:			7/18/2018 11:01:30 AM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Debugger_For_Unity {

	/// <summary>
	/// Billboard, show some usefle information
	/// </summary>
	public partial class Debugger
	{
        [Serializable]
        private sealed partial class Billboard : IWindow
        {
            #region  Attributes and Properties
            /// <summary>
            /// Public Members
            /// </summary>

            /// <summary>
            /// Properties
            /// </summary>

            /// <summary>
            /// Protected Members
            /// </summary>

            /// <summary>
            /// Private Members
            /// </summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;
            private const int MBSize = 1024 * 1024;
            #endregion


            #region Engine Methods

            #endregion


            #region Public Methods

            #endregion


            #region Protected Methods

            #endregion


            #region Private Methods
            private void DrawInfo(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title, GUILayout.Width(TitleWidth));
                    GUILayout.Label(content);
                }
                GUILayout.EndHorizontal();
            }

            private string ConvertResolution(Resolution resolution)
            {
                return string.Format("{0} x {1} @ {2}Hz", resolution.width.ToString(), resolution.height.ToString(), resolution.refreshRate.ToString());
            }
            #endregion


            #region Static Methods

            #endregion

            #region Interface Public Method
            public void OnWindowAwake(params object[] args)
            {
                
            }

            public void OnWindowDestroy()
            {
                
            }

            public void OnWindowDraw()
            {
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.Label("<b>Device</b>");
                        DrawInfo("Name:", SystemInfo.deviceName);
                        DrawInfo("Type:", SystemInfo.deviceType.ToString());
                        DrawInfo("System:", SystemInfo.operatingSystem);
                        DrawInfo("Resolution", ConvertResolution(Screen.currentResolution));
                        DrawInfo("DPI", Screen.dpi.ToString("F2"));
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.Label("<b>Profiler</b>");
                        DrawInfo("Supported:", Profiler.supported.ToString());
                        DrawInfo("Enabled:", Profiler.enabled.ToString());
                        DrawInfo("Enable Binary Log:", Profiler.enableBinaryLog ? string.Format("True, {0}", Profiler.logFile) : "False");
                        DrawInfo("Mono Used Size:", string.Format("{0} MB", (Profiler.GetMonoUsedSizeLong() / (float)MBSize).ToString("F3")));
                        DrawInfo("Mono Heap Size:", string.Format("{0} MB", (Profiler.GetMonoHeapSizeLong() / (float)MBSize).ToString("F3")));
                        DrawInfo("Used Heap Size:", string.Format("{0} MB", (Profiler.usedHeapSizeLong / (float)MBSize).ToString("F3")));
                        DrawInfo("Total Allocated Memory:", string.Format("{0} MB", (Profiler.GetTotalAllocatedMemoryLong() / (float)MBSize).ToString("F3")));
                        DrawInfo("Total Reserved Memory:", string.Format("{0} MB", (Profiler.GetTotalReservedMemoryLong() / (float)MBSize).ToString("F3")));
                        DrawInfo("Total Unused Reserved Memory:", string.Format("{0} MB", (Profiler.GetTotalUnusedReservedMemoryLong() / (float)MBSize).ToString("F3")));
                        DrawInfo("Temp Allocator Size:", string.Format("{0} MB", (Profiler.GetTempAllocatorSize() / (float)MBSize).ToString("F3")));
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
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
        }
        
    }
}