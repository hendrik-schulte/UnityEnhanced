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

            if (!Application.isPlaying) return;

            if (stateManager.GetState()) GUILayout.Label("Current State: " + stateManager.GetState().name);
            else GUILayout.Label("Current State: None");
        }
    }
}