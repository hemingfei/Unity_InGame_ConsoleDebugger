#region Author
/// <summary>--------------------------------------------------
//		Author:			He, Mingfei
//		Namespace:		<Debugger_For_Unity.something>
//		Class:			Debugger.Main
//		Date:			7/18/2018 10:59:28 AM
/// </summary>--------------------------------------------------
#endregion

using System.Collections.Generic;
using UnityEngine;

namespace Debugger_For_Unity
{

    /// <summary>
    /// Class quick explanation
    /// </summary>
    [DisallowMultipleComponent]
    public partial class Debugger : MonoBehaviour
    {
        #region  Attributes and Properties
        /// <summary>
        /// Private Members
        /// </summary>

        //
        // Custom Settings
        //
        [SerializeField]
        private GUISkin m_skin = null; // gui skin if you want

        [SerializeField]
        private GameObject m_maskCanvas = null; // when open debugger window, it can touch the background or not

        [SerializeField]
        private float m_fpsRefreshInterval = 0.5f;
        //
        //Size
        //
        private static Rect DefaultIconRect = new Rect(0f, 0f, 50f, 50f); // size of the icon

        private static readonly Rect DefaultDragRect = new Rect(0f, 0f, float.MaxValue, float.MaxValue); // size of the drag area, set it full

        private static Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f); // size of window

        private static float DefaultWindowScale = 1f; // scale size of icon and window

        //
        // Windows
        //
        private DebuggerManager m_debuggerManager = new DebuggerManager();

        private Fps m_Fps = null;

        /// <summary>
        /// Properties
        /// </summary>
        public bool ShowFullWindow { get; set; }

        public Rect IconRect { get; set; }

        public Rect WindowRect { get; set; }

        public float WindowScale { get; set; }


        #endregion


        #region Engine Methods
        private void Awake()
        {
            // give default values here (C# 4)
            WindowScale = 1;

            // new fps
            m_Fps = new Fps(m_fpsRefreshInterval);

            // check drag items
            if (m_maskCanvas == null)
            {
                return;
            }

            // deactive canvas mask
            if (m_maskCanvas.activeSelf)
            {
                m_maskCanvas.SetActive(false);
            }
        }

        private void Update()
        {
            m_Fps.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnGUI()
        {

            if (m_debuggerManager == null)
            {
                return;
            }

            GUISkin cachedGuiSkin = GUI.skin;
            Matrix4x4 cachedMatrix = GUI.matrix;

            GUI.skin = m_skin;
            GUI.matrix = Matrix4x4.Scale(new Vector3(WindowScale, WindowScale, 1f));
            
            if (ShowFullWindow)
            {
                WindowRect = GUILayout.Window(0, WindowRect, DrawWindow, "<b>DEBUGGER</b>");
                if (m_maskCanvas != null &&!m_maskCanvas.activeSelf)
                {
                    m_maskCanvas.SetActive(true);
                }
            }
            else
            {
                IconRect = GUILayout.Window(0, IconRect, DrawIcon, "<b>DEBUGGER</b>");
                if (m_maskCanvas != null && m_maskCanvas.activeSelf)
                {
                    m_maskCanvas.SetActive(false);
                }
            }

            GUI.matrix = cachedMatrix;
            GUI.skin = cachedGuiSkin;
        }
        #endregion


        #region Public Methods

        #endregion


        #region Protected Methods

        #endregion


        #region Private Methods
        private void RegisterDebuggerWindow(string path, IWindow debuggerWindow, params object[] args)
        {
            m_debuggerManager.RegisterDebuggerWindow(path, debuggerWindow, args);
        }

        private void DrawIcon(int windowId)
        {
            GUI.DragWindow(DefaultDragRect);
            GUILayout.Space(5);
            Color32 color = Color.cyan;

            string title = string.Format("<b>FPS: {0}</b>", m_Fps.CurrentFps.ToString("F2"));
            if (GUILayout.Button(title, GUILayout.Width(100f), GUILayout.Height(50f)))
            {
                ShowFullWindow = true;
            }
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(DefaultDragRect);
            DrawDebuggerWindowGroup(m_debuggerManager.DebuggerWindowRoot);
        }

        private void DrawDebuggerWindowGroup(WindowGroup debuggerWindowGroup)
        {
            if (debuggerWindowGroup == null)
            {
                return;
            }

            List<string> names = new List<string>();
            string[] debuggerWindowNames = debuggerWindowGroup.GetWindowNames();
            for (int i = 0; i < debuggerWindowNames.Length; i++)
            {
                names.Add(string.Format("<b>{0}</b>", debuggerWindowNames[i]));
            }

            if (debuggerWindowGroup == m_debuggerManager.DebuggerWindowRoot)
            {
                names.Add("<b>Close</b>");
            }

            int toolbarIndex = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, names.ToArray(), GUILayout.Height(30f), GUILayout.MaxWidth(Screen.width));
            if (toolbarIndex >= debuggerWindowGroup.WindowCount)
            {
                ShowFullWindow = false;
                return;
            }

            if (debuggerWindowGroup.SelectedIndex != toolbarIndex)
            {
                debuggerWindowGroup.SelectedWindow.OnWindowExit();
                debuggerWindowGroup.SelectedIndex = toolbarIndex;
                debuggerWindowGroup.SelectedWindow.OnWindowEnter();
            }

            WindowGroup subDebuggerWindowGroup = debuggerWindowGroup.SelectedWindow as WindowGroup;
            if (subDebuggerWindowGroup != null)
            {
                DrawDebuggerWindowGroup(subDebuggerWindowGroup);
            }

            if (debuggerWindowGroup.SelectedWindow != null)
            {
                debuggerWindowGroup.SelectedWindow.OnWindowDraw();
            }
        }

        #endregion


        #region Static Methods

        #endregion
    }
}