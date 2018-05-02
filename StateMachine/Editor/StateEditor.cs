#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(State))]
    [CanEditMultipleObjects]
    public class StateEditor : Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty stateManagerProp;

        private void OnEnable()
        {
            m_Script = serializedObject.FindProperty("m_Script");
            stateManagerProp = serializedObject.FindProperty("stateManager");
        }

        public override void OnInspectorGUI()
        {
            var state = target as State;

            GUI.enabled = false;
            EditorGUILayout.ObjectField(m_Script);
            GUI.enabled = true;
            
            serializedObject.Update();

            EditorGUILayout.PropertyField(stateManagerProp);

            if (state.stateManager)
            {
                EditorGUILayout.LabelField("Inherited from StateManager:");

                GUI.enabled = false;
                EditorGUI.indentLevel++;

                EditorGUILayout.Toggle("Instanced", state.stateManager.Instanced);
#if UE_Photon
                EditorGUILayout.Toggle("PUN Sync", state.stateManager.PUNSyncEnabled);
#endif
                EditorGUILayout.Toggle("Log To File", state.stateManager.FileLoggingEnabled);
                EditorGUILayout.Toggle("Log To Console", state.stateManager.ConsoleLoggingEnabled);

                EditorGUI.indentLevel--;
                GUI.enabled = true;
            }


            DrawPropertiesExcluding(serializedObject, "m_Script", "stateManager");

            serializedObject.ApplyModifiedProperties();


            if (!state.stateManager) return;

            InitialState(state);

            if (!Application.isPlaying) return;

            if (state.stateManager.Instanced)
            {
                if (state.stateManager.AllInstancesInEitherState(state))
                {
                    EditorGUILayout.LabelField("All instances are currently in this state.");
                }
                else
                {
                    if (GUILayout.Button("Enter for all Instances"))
                        state.EnterAllInstances();
                }
            }
            else
            {
                if (state.IsActive())
                {
                    EditorGUILayout.LabelField("This state is currently active.");
                }
                else
                {
                    if (GUILayout.Button("Enter"))
                        state.Enter();
                }
            }
        }

        private void InitialState(State state)
        {
            if (state.IsInitialState())
            {
                EditorGUILayout.LabelField("Initial State", "This");
                return;
            }

            var initialState = state.stateManager.InitialState;

            if (initialState) EditorGUILayout.LabelField("Initial State", initialState.name);
            else EditorGUILayout.LabelField("Initial State", "None");
            
            if (GUILayout.Button("Set Initial State"))
            {
                state.stateManager.InitialState = state;
                EditorUtility.SetDirty(state.stateManager);
            }
        }
    }
}
#endif