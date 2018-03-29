using UnityEditor;
using UnityEngine;

namespace UE.UI.Geometry
{
    [CustomEditor(typeof(UIPolygon))]
    public class UIPolygonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var polygon = target as UIPolygon;

            if (GUILayout.Button("Build"))
                polygon.Build();
            
            
            if (GUILayout.Button("Clear"))
                polygon.Clear();
        }
    }
}