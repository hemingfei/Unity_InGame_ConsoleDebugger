#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//  	Class:			Debugger.Fps
//		Date:			7/18/2018 11:00:36 AM
/// </summary>--------------------------------------------------
#endregion

using System;
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
        [Serializable]
        private sealed class Fps
        {
            #region  Attributes and Properties
            /// <summary>
            /// Private Members
            /// </summary>
            [SerializeField]
            private float m_refreshInterval = 0.5f;
            private int m_frames;
            private float m_accumulator;
            private float m_timeLeft;

            /// <summary>
            /// Properties
            /// </summary>
            public float CurrentFps { get; private set; }
            private float RefreshInterval
            {
                get
                {
                    return m_refreshInterval;
                }
                set
                {
                    if (value <= 0f)
                    {
                        return;
                    }
                    m_refreshInterval = value;
                    Reset();
                }
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Be invoked at the Debugger.Main's real MonoBehaviour's Update
            /// </summary>
            /// <param name="deltaTime">Time.deltaTime, elapseSeconds</param>
            /// <param name="unscaledDeltaTime">Time.unscaledDeltatime, realElapseSeconds</param>
            public void Update(float deltaTime, float unscaledDeltaTime)
            {
                m_frames++;
                m_accumulator += unscaledDeltaTime;
                m_timeLeft -= unscaledDeltaTime;
                if (m_timeLeft <= 0f)
                {
                    CurrentFps = m_accumulator > 0f ? m_frames / m_accumulator : 0f;
                    m_frames = 0;
                    m_accumulator = 0f;
                    m_timeLeft += m_refreshInterval;
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
                m_frames = 0;
                m_accumulator = 0f;
                m_timeLeft = 0f;
            }
            #endregion
        }
    }
}