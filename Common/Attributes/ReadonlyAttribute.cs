using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Common
{
    public class ReadonlyAttribute : PropertyAttribute
    {    
        /// <summary>
        /// Using this attribute on a serialized property in Unity will make it read only in the inspector.
        /// </summary>
        public ReadonlyAttribute()
        {
        }
    }
    
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
        public class ReadonlyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
            {
                string valueStr;

                switch (prop.propertyType)
                {
                    case SerializedPropertyType.Integer:
                        valueStr = prop.intValue.ToString();
                        break;
                    case SerializedPropertyType.Boolean:
                        valueStr = prop.boolValue.ToString();
                        break;
                    case SerializedPropertyType.Float:
                        valueStr = prop.floatValue.ToString("0.0000");
                        break;
                    case SerializedPropertyType.String:
                        valueStr = prop.stringValue;
                        break;
                    case SerializedPropertyType.Enum:
                        valueStr = prop.enumDisplayNames[prop.enumValueIndex];
                        break;
                    default:
//                        valueStr = "(not supported)";
                        DrawDisabledProperty(position, prop);
                        return;
                }

                EditorGUI.LabelField(position, label.text, valueStr);
            }

            private void DrawDisabledProperty(Rect position, SerializedProperty prop)
            {
                var previous = GUI.enabled;
                
                GUI.enabled = false;
                EditorGUI.PropertyField(position, prop);
                GUI.enabled = previous;
            }
        }
    
#endif
}