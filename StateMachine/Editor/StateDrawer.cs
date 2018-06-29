#if UNITY_EDITOR
using UE.Common;
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
    public class StateDrawer : ButtonPropertyDrawer<State>
    {
        protected override bool EnableButton(State state)
        {
            return state && state.stateManager && Application.isPlaying;
        }

        protected override void DrawButton(Rect buttonRect, SerializedProperty property, State state)
        {
            var parent = property.GetParent<object>();

            var observer = parent as IInstanceReference;

            var isObserver = observer != null;
            var isInstanced = state.stateManager.Instanced;
                
            bool isActive;
            if (isObserver)
                isActive = state.IsActive(observer.Key);
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
                        state.Enter(observer.Key);
                }
            }
        }
    }
}
#endif