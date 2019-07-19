#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(StateListener), true)]
    [CanEditMultipleObjects]
    public class StateListenerEditor : InstanceObserverEditor
    {
        private SerializedProperty activeStates;

        protected override void InitInspector()
        {
            base.InitInspector();

            activeStates = serializedObject.FindProperty("activeStates");
        }

        protected override void DrawInspector()
        {
            var activeStatesList = activeStates.ToList<State>();

            if (!(activeStatesList.Any() && activeStatesList[0] != null))

                EditorGUILayout.HelpBox("You need to define at least one active state for this listener to work.",
                    MessageType.Error);

            else if (!activeStatesList.StatesShareStateManager())

                EditorGUILayout.HelpBox("Your states do not share the same StateManager. This is not supported.",
                    MessageType.Error);

            base.DrawInspector();
        }

        protected sealed override IEnumerable<string> ExcludeProperties()
        {
            if ((target as StateListener).DrawUnityEventInspector)
                return base.ExcludeProperties();
            else
                return new[] {"OnActivated", "OnDeactivated"};
        }
    }
}
#endif