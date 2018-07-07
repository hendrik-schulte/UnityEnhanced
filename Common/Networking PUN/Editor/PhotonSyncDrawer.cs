#if UNITY_EDITOR
using UE.Common;
using UnityEditor;
using UnityEngine;

namespace UE.PUNNetworking
{
    [CustomPropertyDrawer(typeof(PhotonSync))]
    public class PhotonSyncDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabled = property.FindPropertyRelative("PUNSync");

            EditorGUI.PropertyField(position.GetLine(1), enabled);

            if (enabled.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUI.PropertyField(
                    position.GetLine(2),
                    property.FindPropertyRelative("cachingOptions"));

                EditorGUI.indentLevel--;
                
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 1;

            if (property.FindPropertyRelative("PUNSync").boolValue)
                lines++;
            
                
            return EditorUtil.PropertyHeight(lines);
        }
    }
}
#endif