using UnityEditor;
using UnityEngine;

namespace StateMachine
{
    [CustomEditor(typeof(State))]
    public class StateEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying) return;

            var state = target as State;

            if (state.IsActive())
            {
                GUILayout.Label("This is the current state.");
            }
            else
            {
                if (GUILayout.Button("Enter"))
                    state.Enter();
            }
        }
    }
}