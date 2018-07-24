#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.WindowSize
//		Date:			7/23/2018 4:14:49 PM
/// </summary>--------------------------------------------------
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// Class Explanation
	/// </summary>
	public partial class Debugger
	{
        [Serializable]
        private sealed class WindowSize : IWindow
        {
            #region  Attributes and Properties
            /// <summary>
            /// Properties
            /// </summary>
            public Debugger Debugger { get; set; }

            /// <summary>
            /// Private Members
            /// </summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;
            #endregion

            #region Interface Methods
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
                        GUILayout.BeginHorizontal();
                        {
                            //
                            // width
                            //
                            float width = Debugger.WindowRect.width;
                            GUILayout.Label("Width:", GUILayout.Width(50f));
                            if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                            {
                                width--;
                            }
                            //width = GUILayout.HorizontalSlider(width, 100f, Screen.width - 20f);
                            if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                            {
                                width++;
                            }
                            width = Mathf.Clamp(width, 100f, Screen.width - 20f);
                            if (width != Debugger.WindowRect.width)
                            {
                                Debugger.WindowRect = new Rect(Debugger.WindowRect.x, Debugger.WindowRect.y, width, Debugger.WindowRect.height);
                            }


                            //
                            // height
                            //
                            float height = Debugger.WindowRect.height;
                            GUILayout.Label("", GUILayout.Width(30f));
                            GUILayout.Label("Height:", GUILayout.Width(50f));
                            if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                            {
                                height--;
                            }
                            //height = GUILayout.HorizontalSlider(height, 100f, Screen.height - 20f);
                            if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                            {
                                height++;
                            }
                            height = Mathf.Clamp(height, 100f, Screen.height - 20f);
                            if (height != Debugger.WindowRect.height)
                            {
                                Debugger.WindowRect = new Rect(Debugger.WindowRect.x, Debugger.WindowRect.y, Debugger.WindowRect.width, height);
                            }

                        }
                        GUILayout.EndHorizontal();
                        
                    }
                    GUILayout.EndVertical();


                    GUILayout.BeginVertical("box");
                    {
                        GUILayout.BeginHorizontal();
                        {
                            //
                            // scale
                            //
                            float scale = Debugger.WindowScale;
                            GUILayout.Label("Scale:", GUILayout.Width(50f));
                            if (GUILayout.RepeatButton("-", GUILayout.Width(30f)))
                            {
                                scale -= 0.01f;
                            }
                            //scale = GUILayout.HorizontalSlider(scale, 0.5f, 4f);
                            if (GUILayout.RepeatButton("+", GUILayout.Width(30f)))
                            {
                                scale += 0.01f;
                            }
                            scale = Mathf.Clamp(scale, 0.5f, 5f);
                            if (scale != Debugger.WindowScale)
                            {
                                Debugger.WindowScale = scale;
                            }
                        }
                        GUILayout.EndHorizontal();


                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("1.0x", GUILayout.Height(30f)))
                            {
                                Debugger.WindowScale = 1f;
                            }
                            if (GUILayout.Button("1.5x", GUILayout.Height(30f)))
                            {
                                Debugger.WindowScale = 1.5f;
                            }
                        }
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        {
                            if (GUILayout.Button("0.5x", GUILayout.Height(30f)))
                            {
                                Debugger.WindowScale = 0.5f;
                            }
                            if (GUILayout.Button("2.0x", GUILayout.Height(30f)))
                            {
                                Debugger.WindowScale = 2f;
                            }
                        }
                        GUILayout.EndHorizontal();
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