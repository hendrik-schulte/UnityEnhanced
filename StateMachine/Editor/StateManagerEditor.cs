using UE.Instancing;
using UnityEditor;
using UnityEngine;
#if UE_Photon
using UE.PUNNetworking;
#endif

namespace UE.StateMachine
{
    [CustomEditor(typeof(StateManager))]
    public class StateManagerEditor : InstanciableSOEditor
    {
#if UE_Photon
        private SerializedProperty PUNSync;
        private SerializedProperty CachingOptions;
#endif
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
#if UE_Photon
            PUNSync = serializedObject.FindProperty("PUNSync");
            CachingOptions = serializedObject.FindProperty("CachingOptions");
#endif
        }
        
        protected override void OnInspectorGUITop()
        {
#if UE_Photon
            serializedObject.Update();
            ScriptableObjectEditorUtility.PhotonControl(PUNSync, CachingOptions);
            serializedObject.ApplyModifiedProperties();
#endif
        }
        
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
            EditorGUILayout.LabelField("Name, Key", "Current State");
        }

        protected override void DrawInstance(Object key)
        {
            var stateManager = target as StateManager;

            EditorGUILayout.LabelField(key.name+ ", " + key.GetHashCode(), stateManager.Instance(key).GetState()?.name);
        }         

    }
}