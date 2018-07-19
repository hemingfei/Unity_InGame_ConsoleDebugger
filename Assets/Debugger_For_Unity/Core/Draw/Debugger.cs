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

        [SerializeField]
        private float m_initWindowScale = 1;

        [SerializeField]
        private Texture m_infoImage = null;

        [SerializeField]
        private Texture m_warningImage = null;

        [SerializeField]
        private Texture m_errorImage = null;
        //
        //Size
        //
        private static Rect DefaultIconRect = new Rect(0f, 0f, 50f, 50f); // size of the icon

        private static readonly Rect DefaultDragRect = new Rect(0f, 0f, float.MaxValue, 20); // size of the drag area, set it full

        private static Rect DefaultWindowRect = new Rect(10f, 10f, 640f, 480f); // size of window

        private static float DefaultWindowScale = 1f; // scale size of icon and window

        //
        // Windows
        //
        private DebuggerManager m_debuggerManager = new DebuggerManager();

        private Fps m_fps = null;

        private float m_uiAdaptiveScale = 1;

        private Console m_console = new Console();

        /// <summary>
        /// Properties
        /// </summary>
        public bool ShowFullWindow { get; set; } //= false;

        public Rect IconRect { get; set; }

        public Rect WindowRect { get; set; }

        public float WindowScale { get; set; } //= 1;


        #endregion


        #region Engine Methods
        private void Awake()
        {
            // give default values here (C# 4)
            WindowScale = m_initWindowScale;

            ShowFullWindow = false;

            IconRect = new Rect(0, 0, 50, 50);

            //WindowRect = new Rect(0, 0, 1000, 500);

            // UI adaptive
            m_uiAdaptiveScale = (Screen.width / 960.0f); // using 960 * 540 as default native resolution

            // new fps
            m_fps = new Fps(m_fpsRefreshInterval);

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

        private void Start()
        {
            // register the windows
            RegisterDebuggerWindow("Console", m_console);
            for(int i = 0; i < 20; i++)
            {
                Debug.Log("1");
                Debug.LogWarning("2");
                Debug.LogError("3");
            }
            
        }

        private void Update()
        {
            // update fps, the unscaledDeltaTime is used
            m_fps.Update(Time.deltaTime, Time.unscaledDeltaTime);
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log(Random.Range(1,5));
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.LogWarning(Random.Range(1, 5));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogError(Random.Range(1, 5));
            }
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
            GUI.matrix = Matrix4x4.Scale(new Vector3(WindowScale * m_uiAdaptiveScale, WindowScale * m_uiAdaptiveScale, 1f));
            
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
                GUIStyle guiStyle = new GUIStyle();
                guiStyle.fontSize = 20;
                guiStyle.alignment = TextAnchor.LowerCenter;
                Color32 color = Color.white;
                string title = string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_fps.CurrentFps.ToString("F2"));

                IconRect = GUILayout.Window(0, IconRect, DrawIcon, string.Format("<b>{0}</b>", title));
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
            // drag at the top area
            GUI.DragWindow(DefaultDragRect);

            // string stype used below code
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 10;
            guiStyle.alignment = TextAnchor.LowerCenter;
            Color32 color = Color.white;

            // write a tip told user that the above number is FPS
            GUILayout.Label(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), "FPS"), guiStyle);

            // add the log info warning error tips at bottom
            m_console.RefreshCount();

            // draw the log count
            GUILayout.BeginVertical("box");
            {
                // info

                color = Color.white;
                GUIContent info = new GUIContent(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_console.InfoCount.ToString()), m_infoImage);
                if (GUILayout.Button(info, guiStyle, GUILayout.Width(30f), GUILayout.Height(20f)))
                {
                    ShowFullWindow = true;
                }


                // warning

                color = Color.yellow;
                GUIContent warning = new GUIContent(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_console.WarningCount.ToString()), m_warningImage);
                if (GUILayout.Button(warning, guiStyle, GUILayout.Width(30f), GUILayout.Height(20f)))
                {
                    ShowFullWindow = true;
                }


                // error

                color = Color.red;
                GUIContent error = new GUIContent(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_console.ErrorCount.ToString()), m_errorImage);
                if (GUILayout.Button(error, guiStyle, GUILayout.Width(30f), GUILayout.Height(20f)))
                {
                    ShowFullWindow = true;
                }

            }
            GUILayout.EndVertical();
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