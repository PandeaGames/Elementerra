using PandeaGames.Runtime.Gameplay.AI;
using UnityEditor;
using UnityEngine;

namespace PandeaGames.Editor
{
    [CustomEditor(typeof(PandeaFSM))]
    public class PandeaFSMEditor : UnityEditor.Editor
    {
        public static bool ShowDebugLog
        {
            get { return PlayerPrefs.GetInt("PandeaFSMEditor_ShowDebugLog", 0) == 1; }
            set { PlayerPrefs.SetInt("PandeaFSMEditor_ShowDebugLog", value ? 1:0); }
        }
        
        private Vector2 m_scrollArea;
        
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                ShowDebugLog = EditorGUILayout.Foldout(ShowDebugLog, "Editor Log");

                if (ShowDebugLog)
                {
                    PandeaFSM fsm = target as PandeaFSM;
                    EditorGUILayout.BeginHorizontal();
                    m_scrollArea = EditorGUILayout.BeginScrollView(m_scrollArea,
                        GUILayout.Width(EditorGUIUtility.currentViewWidth), GUILayout.Height(400));
                    for (int i = 0; i < fsm.StateChangeLog.Count; i++)
                    {
                        EditorGUILayout.LabelField($"[{i}]: {fsm.StateChangeLog[i]}");
                    }

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                }
            }

            base.OnInspectorGUI();
        }
    }
}