using UE.Common;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Attributes
 {
     public class SpritePreviewAttribute : PropertyAttribute
     {    
         /// <summary>
         /// This attribute on a sprite field in Unity to show a preview in the inspector.
         /// </summary>
         public SpritePreviewAttribute()
         {
         }
     }
     
#if UNITY_EDITOR
     [CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
     [CustomPropertyDrawer(typeof(Sprite))] //Enable this to apply drawer to all sprites.
     public class SpritePropertyDrawer : PropertyDrawer
     {
         private const float _textureSize = 65;

         public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
         {
             return  (prop.objectReferenceValue != null ? _textureSize : base.GetPropertyHeight(prop, label));
         }

         public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
         {
             EditorGUI.BeginProperty(position, label, prop);

             if (prop.objectReferenceValue != null)
             {
                 var propertyRect = position.Offset(0,- _textureSize - 2,0,0);
                 propertyRect.height = EditorGUIUtility.singleLineHeight;
                 EditorGUI.PropertyField(propertyRect, prop);
                 
                 position.x = propertyRect.x + propertyRect.width + 2;
                 position.width = _textureSize;
                 position.height = _textureSize;

                 
                 prop.objectReferenceValue = 
                     EditorGUI.ObjectField(position, prop.objectReferenceValue, typeof(Sprite));
             }
             else
             {
                 EditorGUI.PropertyField(position, prop);
             }

             EditorGUI.EndProperty();
         }
     }
#endif
 }