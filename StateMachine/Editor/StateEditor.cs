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
#if UE_Photon
                EditorGUILayout.Toggle("PUN Sync", state.stateManager.PUNSyncEnabled);
#endif
                GUI.enabled = true;
            }

            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, "m_Script");

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