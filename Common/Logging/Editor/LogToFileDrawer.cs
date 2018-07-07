#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Common
{
    [CustomPropertyDrawer(typeof(LogToFile))]
    public class LogToFileDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabled = property.FindPropertyRelative("logToFile");

            EditorGUI.PropertyField(position.GetLine(1), enabled);

            if (enabled.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUI.PropertyField(
                    position.GetLine(2),
                    property.FindPropertyRelative("FileName"));

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 1;

            if (property.FindPropertyRelative("logToFile").boolValue)
                lines++;
                
            return EditorUtil.PropertyHeight(lines);
        }
    }
}
#endif