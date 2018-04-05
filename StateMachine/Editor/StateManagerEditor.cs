using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(StateManager))]
    public class StateManagerEditor : InstanciableSOEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var stateManager = target as StateManager;

            
            if (stateManager.InitialState && GUILayout.Button("Set No Initial State"))
            {
                stateManager.InitialState = null;
                EditorUtility.SetDirty(stateManager);
            }

            if (!Application.isPlaying || stateManager.Instanced) return;

            if (stateManager.GetState()) GUILayout.Label("Current State: " + stateManager.GetState().name);
            else GUILayout.Label("Current State: None");
        }

        protected override void DrawInstanceListHeader()
        {
            EditorGUILayout.LabelField("Key", "Current State", EditorStyles.boldLabel);
        }

        protected override void DrawInstance(Object key)
        {
            var stateManager = target as StateManager;

            EditorGUILayout.LabelField(key.name, stateManager.Instance(key).GetState()?.name);
        }
    }
}