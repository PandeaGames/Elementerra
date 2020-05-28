using System;
using PandeaGames.Runtime.Gameplay.AI;
using UnityEngine;
using UnityEditor;

namespace PandeaGames.Editor
{
    [CustomEditor(typeof(AbstractPandeaStateCondition), editorForChildClasses:true)]
    public class PandeaConditionEditor : UnityEditor.Editor
    {
        public static bool ShowDebugLog
        {
            get { return PlayerPrefs.GetInt("PandeaConditionEditor_ShowDebugLog", 0) == 1; }
            set { PlayerPrefs.SetInt("PandeaConditionEditor_ShowDebugLog", value ? 1:0); }
        }
        
        private Vector2 m_scrollArea;
        
        
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                ShowDebugLog = EditorGUILayout.Foldout(ShowDebugLog, "Editor Log");

                if (ShowDebugLog)
                {
                    AbstractPandeaStateCondition condition = target as AbstractPandeaStateCondition;
                    EditorGUILayout.BeginHorizontal();
                    m_scrollArea = EditorGUILayout.BeginScrollView(m_scrollArea, true, true, GUILayout.Width(EditorGUIUtility.currentViewWidth), GUILayout.Height(200));
                    for (int i = Math.Max(0, condition.EvaluationLog.Count - 1000); i < condition.EvaluationLog.Count; i++)
                    {
                        EditorGUILayout.LabelField($"[{i}]: {condition.EvaluationLog[i]}");
                    }
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}