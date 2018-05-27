
using UE.Common;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UE.UI
{
    public static class Gizmo
    {
        /// <summary>
        /// Draws the given string at the given position.
        /// Adapted from https://gist.github.com/Arakade/9dd844c2f9c10e97e3d0
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="color"></param>
        public static void DrawWorldSpaceString(string text, Vector3 worldPos, Color? color = null)
        {
#if UNITY_EDITOR
            Handles.BeginGUI();

            var restoreColor = GUI.color;

            if (color.HasValue) GUI.color = color.Value;
            var view = SceneView.currentDrawingSceneView;
            
            if (view == null)
                return;
            
            var screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width ||
                screenPos.z < 0)
            {
                GUI.color = restoreColor;
                Handles.EndGUI();
                return;
            }

            var size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y),
                text);
            GUI.color = restoreColor;
            Handles.EndGUI();
#endif
        }
    }
}