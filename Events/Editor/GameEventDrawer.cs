#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    /// <summary>
    /// This adds a property drawer for GameEvent fields that allows you to fire that event directly.
    /// </summary>
    [CustomPropertyDrawer(typeof(GameEvent))]
    public class GameEventDrawer : PropertyDrawer
    {
        private GUIStyle buttonStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button);
                buttonStyle.fixedWidth = 45;
                buttonStyle.fixedHeight = GetPropertyHeight(property, label);
            }

            var gameEvent = property.objectReferenceValue as GameEvent;

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            if (gameEvent)
            {
                position = EditorGUI.PrefixLabel(position, label);

                var buttonRect = new Rect(position);
                buttonRect.width = buttonStyle.fixedWidth + buttonStyle.margin.right;
                position.xMin = buttonRect.xMax;

                var previous = GUI.enabled;
                GUI.enabled = Application.isPlaying;
                
                if (GUI.Button(buttonRect, "Raise", buttonStyle))
                {
                    var parent = property.GetParent<InstanceObserver>();

                    if (parent == null)
                        gameEvent.Raise();
                    else
                        gameEvent.Raise(parent.key);
                }
                
                GUI.enabled = previous;

                EditorGUI.PropertyField(position, property, GUIContent.none);
            }
            else EditorGUI.PropertyField(position, property);

            EditorGUI.EndProperty();
        }
    }
}
#endif