using UnityEngine;

namespace Terra.Utils
{
    public static class DebugUtils
    {   
        private static GUIStyle guiStyle = new GUIStyle(GUIStyle.none);
        static public void DrawString(string text, Vector3 worldPos, Color colour, int fontSize = 10, float offsetX = 0, float offsetY = 0 ) {
            #if UNITY_EDITOR
            UnityEditor.Handles.BeginGUI();
            guiStyle.fontSize = fontSize;
            var restoreColor = GUI.color;
            guiStyle.normal.textColor = colour;

            var view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
 
            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }
 
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2) + offsetX, -screenPos.y + view.position.height + offsetY, size.x, size.y), text, guiStyle);
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
            #endif
        }
    }
}