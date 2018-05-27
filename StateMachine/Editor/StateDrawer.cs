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

            if (state && state.stateManager && Application.isPlaying)
            {
                position = EditorGUI.PrefixLabel(position, label);

                var buttonRect = new Rect(position);
                buttonRect.width = buttonStyle.fixedWidth + buttonStyle.margin.right;
                position.xMin = buttonRect.xMax;

                var parent = property.GetParent();

                var observer = parent as InstanceObserver;

                var isObserver = observer != null;
                var isInstanced = state.stateManager.Instanced;
                
                bool isActive;
                if (isObserver)
                    isActive = state.IsActive(observer.key);
                else if (isInstanced) isActive = state.AllInstancesActive();
                    else isActive = state.IsActive();

                if (isActive)
                {
                    EditorGUI.LabelField(buttonRect, "Active");
                }
                else
                {
                    if (GUI.Button(buttonRect, "Enter", buttonStyle))
                    {
                        if (!isObserver)
                            if (isInstanced)
                                state.EnterAllInstances();
                            else
                                state.Enter();
                        else
                            state.Enter(observer.key);
                    }
                }

                EditorGUI.PropertyField(position, property, GUIContent.none);
            }
            else EditorGUI.PropertyField(position, property);

            EditorGUI.EndProperty();
        }
    }
}
#endif