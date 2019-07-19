using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UE.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LayerFieldAttribute : PropertyAttribute
    {
        /// <summary>
        /// Add this attribute to an int field to draw it as a layer dropdown in the inspector.
        /// </summary>
        public LayerFieldAttribute()
        {
        }
    }
    
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LayerFieldAttribute))]
    public class LayerFieldDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            return base.GetPropertyHeight(prop, label);
        }


        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, prop);

            if (prop.propertyType == SerializedPropertyType.Integer)
            {
                prop.intValue = EditorGUI.LayerField(position, label.text, prop.intValue);
            }
            else
            {
                EditorGUI.HelpBox(position, "LayerField only works with int properties!", MessageType.Error);
            }
            
            EditorGUI.EndProperty();
        }
    }
#endif
}