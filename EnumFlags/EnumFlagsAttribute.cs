//This is taken from:
//http://answers.unity3d.com/questions/486694/default-editor-enum-as-flags-.html


//using UnityEditor;

//using UnityEditor;
using UnityEngine;

namespace UnityFlags
{
    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute()
        {
        }
    }

#if UNITY_EDITOR

    [UnityEditor.CustomPropertyDrawer(typeof (EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : UnityEditor.PropertyDrawer
    {
        //        public override void OnGUI(Rect _position, UnityEditor.SerializedProperty _property, GUIContent _label)
        //        {
        //            _property.intValue = UnityEditor.EditorGUI.MaskField(_position, _label, _property.intValue,
        //                _property.enumNames);
        //        }
        public override void OnGUI(Rect _position, UnityEditor.SerializedProperty _property, GUIContent _label)
        {
            // Change check is needed to prevent values being overwritten during multiple-selection
            UnityEditor.EditorGUI.BeginChangeCheck();
            int newValue = UnityEditor.EditorGUI.MaskField(_position, _label, _property.intValue, _property.enumNames);
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                _property.intValue = newValue;
            }
        }

    }
#endif

}