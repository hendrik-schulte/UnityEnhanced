#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// This adds a property drawer for State fields that allows you to enter that state directly.
    /// </summary>
    [CustomPropertyDrawer(typeof(State))]
    public class StateDrawer : PropertyDrawer
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

            var state = property.objectReferenceValue as State;

            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            if (state && Application.isPlaying)
            {
                position = EditorGUI.PrefixLabel(position, label);

                var buttonRect = new Rect(position);
                buttonRect.width = buttonStyle.fixedWidth + buttonStyle.margin.right;
                position.xMin = buttonRect.xMax;

                var parent = property.GetParent<InstanceObserver>();

                var isActive = state.IsActive(parent?.key);

                if (isActive)
                {
                    EditorGUI.LabelField(buttonRect, "Active");
                }
                else
                {
//                    var previous = GUI.enabled;
//                    GUI.enabled =  && (parent == null);
                
                    if (GUI.Button(buttonRect, "Enter", buttonStyle))
                    {
                        if (parent == null)
                            state.Enter();
                        else
                            state.Enter(parent.key);
                    }
                
//                    GUI.enabled = previous;
                }
                
                

                EditorGUI.PropertyField(position, property, GUIContent.none);
            }
            else EditorGUI.PropertyField(position, property);

            EditorGUI.EndProperty();
        }
    }
}
#endif