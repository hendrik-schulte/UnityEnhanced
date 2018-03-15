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

            var state = target as State;

        	if (!state.stateManager) return;

            InitialState(state);

            if (!Application.isPlaying) return;


            if (state.IsActive())
            {
                GUILayout.Label("This state is currently active.");
            }
            else
            {
                if (GUILayout.Button("Enter"))
                    state.Enter();
            }
        }

        private void InitialState(State state)
        {
            if (state.IsInitialState())
            {
                GUILayout.Label("This is the initial state of this system.");
                return;
            }

            if (GUILayout.Button("Set Initial State"))
            {

                state.stateManager.InitialState = state;
                EditorUtility.SetDirty(state.stateManager);
            }
//                state.SetAsInitialState();

            var initialState = state.stateManager.InitialState;

            if(initialState) GUILayout.Label("Initial State: " + initialState.name);
            else GUILayout.Label("Initial State: None");
        }
    }
}