#if UNITY_EDITOR
using System;
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer : ReferenceDrawer
    {
        protected override void DrawConstantProperty(
            Rect position, 
            SerializedProperty property,
            SerializedProperty constantValue)
        {
            if (property.HasAttribute<RangeAttribute>())
            {
                var range = property.GetAttributes<RangeAttribute>()[0] as RangeAttribute;
                
                EditorGUI.IntSlider(position, constantValue, 
                    Convert.ToInt32(range.min), 
                    Convert.ToInt32(range.max), GUIContent.none);
            }
            else base.DrawConstantProperty(position, property, constantValue);
        }
    }
}
#endif