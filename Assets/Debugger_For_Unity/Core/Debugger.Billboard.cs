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
using UnityEngine.SceneManagement;

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

            private const int ShowSampleCount = 300;

            private DateTime m_SampleTime = DateTime.MinValue;
            private long m_SampleSize = 0;
            private long m_DuplicateSampleSize = 0;
            private int m_DuplicateSimpleCount = 0;
            private List<Sample> m_Samples = new List<Sample>();
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

            private void TakeSample()
            {
                m_SampleTime = DateTime.Now;
                m_SampleSize = 0L;
                m_DuplicateSampleSize = 0L;
                m_DuplicateSimpleCount = 0;
                m_Samples.Clear();

                GameObject[] sampleRoots = SceneManager.GetActiveScene().GetRootGameObjects();
                HashSet<GameObject> sampleHs = new HashSet<GameObject>();
                foreach (var sample in sampleRoots)
                {
                    sampleHs = Traverse(sample, sampleHs);
                }

                GameObject[] samples = new GameObject[sampleHs.Count];
                sampleHs.CopyTo(samples);

                for (int i = 0; i < samples.Length; i++)
                {
                    long sampleSize = 0L;
                    sampleSize = Profiler.GetRuntimeMemorySizeLong(samples[i]);
                    m_SampleSize += sampleSize;
                    m_Samples.Add(new Sample(samples[i].name, samples[i].GetType().Name, sampleSize));
                }

                m_Samples.Sort(SampleComparer);

                for (int i = 1; i < m_Samples.Count; i++)
                {
                    if (m_Samples[i].Name == m_Samples[i - 1].Name && m_Samples[i].Type == m_Samples[i - 1].Type && m_Samples[i].Size == m_Samples[i - 1].Size)
                    {
                        m_Samples[i].Highlight = true;
                        m_DuplicateSampleSize += m_Samples[i].Size;
                        m_DuplicateSimpleCount++;
                    }
                }
            }

            private string GetSizeString(long size)
            {
                if (size < 1024L)
                {
                    return string.Format("{0} Bytes", size.ToString());
                }

                if (size < 1024L * 1024L)
                {
                    return string.Format("{0} KB", (size / 1024f).ToString("F2"));
                }

                if (size < 1024L * 1024L * 1024L)
                {
                    return string.Format("{0} MB", (size / 1024f / 1024f).ToString("F2"));
                }

                if (size < 1024L * 1024L * 1024L * 1024L)
                {
                    return string.Format("{0} GB", (size / 1024f / 1024f / 1024f).ToString("F2"));
                }

                return string.Format("{0} TB", (size / 1024f / 1024f / 1024f / 1024f).ToString("F2"));
            }

            private int SampleComparer(Sample a, Sample b)
            {
                int result = b.Size.CompareTo(a.Size);
                if (result != 0)
                {
                    return result;
                }

                result = a.Type.CompareTo(b.Type);
                if (result != 0)
                {
                    return result;
                }

                return a.Name.CompareTo(b.Name);
            }

            private HashSet<GameObject> Traverse(GameObject obj, HashSet<GameObject> samples)
            {

                foreach (Transform child in obj.transform)
                {

                    Traverse(child.gameObject, samples);  
                }
                samples.Add(obj);
                return samples;
            }
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


                    GUILayout.BeginVertical("box");
                    {
                        string typeName = typeof(GameObject).Name;
                        if (GUILayout.Button(string.Format("Current {0}s in Hierarchy ", typeName), GUILayout.Height(30f)))
                        {
                            TakeSample();
                        }

                        if (m_SampleTime <= DateTime.MinValue)
                        {

                        }
                        else
                        {
                            if (m_DuplicateSimpleCount > 0)
                            {
                                GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3}, while {4} {1}s ({5}) might be duplicated.</b>", m_Samples.Count.ToString(), typeName, GetSizeString(m_SampleSize), m_SampleTime.ToString("yyyy-MM-dd HH:mm:ss"), m_DuplicateSimpleCount.ToString(), GetSizeString(m_DuplicateSampleSize)));
                            }
                            else
                            {
                                GUILayout.Label(string.Format("<b>{0} {1}s ({2}) obtained at {3}.</b>", m_Samples.Count.ToString(), typeName, GetSizeString(m_SampleSize), m_SampleTime.ToString("yyyy-MM-dd HH:mm:ss")));
                            }

                            if (m_Samples.Count > 0)
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(string.Format("<b>{0} Name</b>", typeName));
                                    GUILayout.Label("<b>Type</b>", GUILayout.Width(240f));
                                    GUILayout.Label("<b>Size</b>", GUILayout.Width(80f));
                                }
                                GUILayout.EndHorizontal();
                            }

                            int count = 0;
                            for (int i = 0; i < m_Samples.Count; i++)
                            {
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(m_Samples[i].Highlight ? string.Format("<color=yellow>{0}</color>", m_Samples[i].Name) : m_Samples[i].Name);
                                    GUILayout.Label(m_Samples[i].Highlight ? string.Format("<color=yellow>{0}</color>", m_Samples[i].Type) : m_Samples[i].Type, GUILayout.Width(240f));
                                    GUILayout.Label(m_Samples[i].Highlight ? string.Format("<color=yellow>{0}</color>", GetSizeString(m_Samples[i].Size)) : GetSizeString(m_Samples[i].Size), GUILayout.Width(80f));
                                }
                                GUILayout.EndHorizontal();

                                count++;
                                if (count >= ShowSampleCount)
                                {
                                    break;
                                }
                            }
                        }
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


        private sealed class Sample
        {
            #region  Attributes and Properties
            public string Name { get; private set; }

            public string Type { get; private set; }

            public long Size { get; private set; }

            public bool Highlight { get; set; }
            #endregion

            #region Public Method
            public Sample(string name, string type, long size)
            {
                Name = name;
                Type = type;
                Size = size;
                Highlight = false;
            }
            #endregion
        }
    }
}