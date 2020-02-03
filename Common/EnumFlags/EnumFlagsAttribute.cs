//This is adapted from:
//http://answers.unity3d.com/questions/486694/default-editor-enum-as-flags-.html

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UE.UnityFlags
{
    /// <summary>
    /// This attribute can be used to display an enum mask widget covering the options of this enum.
    /// </summary>
    public class EnumFlagsAttribute : PropertyAttribute
    {
        /// <inheritdoc cref="EnumFlagsAttribute"/>
        public EnumFlagsAttribute()
        {
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof (EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            // Change check is needed to prevent values being overwritten during multiple-selection
            EditorGUI.BeginChangeCheck();
            int newValue = EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
            if (EditorGUI.EndChangeCheck())
            {
                _property.intValue = newValue;
            }
        }
    }
#endif
}