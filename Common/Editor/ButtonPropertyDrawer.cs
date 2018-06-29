#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Common
{
    /// <summary>
    /// This property drawer allows to combine a property field with a button next to it.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ButtonPropertyDrawer<T> : PropertyDrawer where T : class
    {
        protected GUIStyle buttonStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.fixedWidth = 45;
                buttonStyle.fixedHeight = GetPropertyHeight(property, label);
            }
            
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            var reference = property.objectReferenceValue as T;

            
            if (EnableButton(reference))
            {
                position = EditorGUI.PrefixLabel(position, label);

                var buttonRect = new Rect(position);
                buttonRect.width = buttonStyle.fixedWidth + buttonStyle.margin.right;
                position.xMin = buttonRect.xMax;
                
                DrawButton(buttonRect, property, reference);
                
                EditorGUI.PropertyField(position, property, GUIContent.none);
            }
            else EditorGUI.PropertyField(position, property, label);

            EditorGUI.EndProperty();
        }

        protected abstract bool EnableButton(T reference);

        protected abstract void DrawButton(Rect buttonRect, SerializedProperty property, T reference);
    }
}
#endif