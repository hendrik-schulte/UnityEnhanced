//This is taken from:
//http://answers.unity3d.com/questions/486694/default-editor-enum-as-flags-.html

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif
using UnityEngine;

namespace UE.UnityFlags
{
    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute()
        {
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof (EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        //        public override void OnGUI(Rect _position, UnityEditor.SerializedProperty _property, GUIContent _label)
        //        {
        //            _property.intValue = UnityEditor.EditorGUI.MaskField(_position, _label, _property.intValue,
        //                _property.enumNames);
        //        }
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