using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(State))]
    [CanEditMultipleObjects]
    public class StateEditor : Editor
    {
        private SerializedProperty m_Script;
        
        private void OnEnable()
        {
            m_Script = serializedObject.FindProperty("m_Script");
        }
        
        public override void OnInspectorGUI()
        {
            var state = target as State;

            GUI.enabled = false;
            EditorGUILayout.ObjectField(m_Script);
            GUI.enabled = true;
            
            if (state.stateManager)
            {            
                GUI.enabled = false;
                EditorGUILayout.Toggle("Instanced", state.stateManager.Instanced);
                GUI.enabled = true;
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");


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
            
            var initialState = state.stateManager.InitialState;

            if(initialState) GUILayout.Label("Initial State: " + initialState.name);
            else GUILayout.Label("Initial State: None");
        }
    }
}