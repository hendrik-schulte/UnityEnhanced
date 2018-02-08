using ExtensionMethods;
using UnityEditor;
using UnityEngine;

namespace MathEx.Editor
{
    [CustomPropertyDrawer(typeof(Matrix3x3))]
    public class Matrix3x3Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

//            var thirdColumn = position.width / 3;
//            
//            // Calculate rects
//            var m00Rect = new Rect(position.x, position.y, thirdColumn, position.height);
//            var m01Rect = new Rect(position.x + thirdColumn, position.y, thirdColumn, position.height);
//            var m02Rect = new Rect(position.x + 2 * thirdColumn, position.y, thirdColumn, position.height);
//
//            var matrix = (Matrix3x3) property.GetTargetObjectOfProperty();
//            
//            matrix.m00 = EditorGUI.FloatField(m00Rect, matrix.m00);
//            matrix.m01 = EditorGUI.FloatField(m01Rect, matrix.m01);
//            matrix.m02 = EditorGUI.FloatField(m02Rect, matrix.m02);

            EditorGUI.PropertyField(position, property.FindPropertyRelative("_mat4x4"), GUIContent.none);
            
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
//            EditorGUI.PropertyField(leftRect, property.FindPropertyRelative("m00"), GUIContent.none);
//            EditorGUI.PropertyField(midRect, property.FindPropertyRelative("m01"), GUIContent.none);
//            EditorGUI.PropertyField(rightRect, property.FindPropertyRelative("m02"), GUIContent.none);
            
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("_mat4x4"));
        }
    }
}