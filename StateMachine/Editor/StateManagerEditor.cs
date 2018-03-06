using UnityEditor;
using UnityEngine;

namespace StateMachine
{
    [CustomEditor(typeof(StateManager))]
    public class StateManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var stateManager = target as StateManager;

            if (stateManager.InitialState && GUILayout.Button("Set No Initial State"))
                stateManager.InitialState = null;

//            if (!Application.isPlaying) return;

            if (stateManager.State) GUILayout.Label("Current State: " + stateManager.State.name);
            else GUILayout.Label("Current State: None");
        }
    }
}