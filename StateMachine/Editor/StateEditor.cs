#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(State))]
    [CanEditMultipleObjects]
    public class StateEditor : ReorderableArrayInspector
    {
        private SerializedProperty m_Script;

        protected override void InitInspector()
        {
            base.InitInspector();

            alwaysDrawInspector = true;

            m_Script = serializedObject.FindProperty("m_Script");
        }

        protected override void DrawInspector()
        {
            var state = target as State;

            GUI.enabled = false;
            EditorGUILayout.ObjectField(m_Script);
            GUI.enabled = true;

            serializedObject.Update();

            DrawProperty("stateManager");

            if (!state.stateManager)
                EditorGUILayout.HelpBox(
                    "You need to assign a State Manager asset or your state is useless.",
                    MessageType.Warning);

            DrawPropertiesExcept("m_Script", "stateManager");

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