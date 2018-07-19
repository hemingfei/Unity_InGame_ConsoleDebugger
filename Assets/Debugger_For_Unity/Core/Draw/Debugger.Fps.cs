#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//  	Class:			Debugger.Fps
//		Date:			7/18/2018 11:00:36 AM
/// </summary>--------------------------------------------------
#endregion

using UnityEngine;

namespace Debugger_For_Unity {

	/// <summary>
	/// Partial Class
	/// </summary>
	public partial class Debugger
	{
        /// <summary>
        /// Fps
        /// </summary>
        private sealed class Fps
        {
            #region  Attributes and Properties
            /// <summary>
            /// Private Members
            /// </summary>
            private float m_UpdateInterval;
            private int m_Frames;
            private float m_Accumulator;
            private float m_TimeLeft;

            /// <summary>
            /// Properties
            /// </summary>
            public float CurrentFps { get; private set; }
            private float UpdateInterval
            {
                get
                {
                    return m_UpdateInterval;
                }
                set
                {
                    if (value <= 0f)
                    {
                        Debug.LogError("Update interval should be positive value. Current is " + value);
                        return;
                    }
                    m_UpdateInterval = value;
                    Reset();
                }
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Constructor
            /// Init the value of Fps refresh interval
            /// </summary>
            /// <param name="updateInterval"> the interval seconds to refresh the current FPS</param>
            public Fps(float updateInterval = 0.5f)
            {
                UpdateInterval = updateInterval;
            }

            /// <summary>
            /// Be invoked at the Debugger.Main's real MonoBehaviour's Update
            /// </summary>
            /// <param name="deltaTime">Time.deltaTime, elapseSeconds</param>
            /// <param name="unscaledDeltaTime">Time.unscaledDeltatime, realElapseSeconds</param>
            public void Update(float deltaTime, float unscaledDeltaTime)
            {
                m_Frames++;
                m_Accumulator += unscaledDeltaTime;
                m_TimeLeft -= unscaledDeltaTime;
                if (m_TimeLeft <= 0f)
                {
                    CurrentFps = m_Accumulator > 0f ? m_Frames / m_Accumulator : 0f;
                    m_Frames = 0;
                    m_Accumulator = 0f;
                    m_TimeLeft += m_UpdateInterval;
                }
            }
            #endregion

            #region Private Methods
            /// <summary>
            /// Reset the values of timer
            /// </summary>
            private void Reset()
            {
                CurrentFps = 0f;
                m_Frames = 0;
                m_Accumulator = 0f;
                m_TimeLeft = 0f;
            }
            #endregion
        }
    }
}