#if UNITY_EDITOR
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.StateMachine
{
    [CustomEditor(typeof(StateManager))]
    public class StateManagerEditor : InstanciableSOEditor
    {
        protected override void OnInspectorGUITop()
        {
        }
        
        protected override void DrawInspector()
        {
            base.DrawInspector();

            var stateManager = target as StateManager;

            
            if (stateManager.InitialState && GUILayout.Button("Set No Initial State"))
            {
                stateManager.InitialState = null;
                EditorUtility.SetDirty(stateManager);
            }

            if (!Application.isPlaying || stateManager.Instanced) return;

            if (stateManager.GetState()) GUILayout.Label("Current State: " + stateManager.GetState().name);
            else GUILayout.Label("Current State: None");
            
            if (stateManager.HasPreviousState() && GUILayout.Button("Back"))
                    stateManager.Back();
                
        }

        protected override void DrawInstanceListHeader()
        {
            EditorGUILayout.LabelField("Name, Key", "Current State");
        }

        protected override void DrawInstance(Object key)
        {
            var stateManager = target as StateManager;

            EditorGUILayout.LabelField(key.name+ ", " + 
                                       key.GetHashCode(), 
                stateManager.GetState(key)?.name);
        }    
    }
}
#endif