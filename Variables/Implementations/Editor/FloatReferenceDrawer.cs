#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer : ReferenceDrawer
    {
        protected override void DrawConstantProperty(
            Rect position, 
            SerializedProperty property,
            SerializedProperty constantValue)
        {
            if (property.HasAttribute<RangeAttribute>())
            {
                var range = property.GetAttributes<RangeAttribute>()[0] as RangeAttribute;
                EditorGUI.Slider(position, constantValue, range.min, range.max, GUIContent.none);
            }
            else base.DrawConstantProperty(position, property, constantValue);
        }
    }
}
#endif