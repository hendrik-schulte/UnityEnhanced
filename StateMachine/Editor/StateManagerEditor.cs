using UE.Common;
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
        private SerializedProperty LogToFile;
        private SerializedProperty FileName;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
#if UE_Photon
            PUNSync = serializedObject.FindProperty("PUNSync");
            CachingOptions = serializedObject.FindProperty("cachingOptions");
#endif
            LogToFile = serializedObject.FindProperty("LogToFile");
            FileName = serializedObject.FindProperty("FileName");
        }
        
        protected override void OnInspectorGUITop()
        {
#if UE_Photon
            serializedObject.Update();
            PhotonEditorUtility.PhotonControl(PUNSync, CachingOptions);
            serializedObject.ApplyModifiedProperties();
#endif
            serializedObject.Update();
            FileLogger.LoggerControl(LogToFile, FileName);
            serializedObject.ApplyModifiedProperties();
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

            EditorGUILayout.LabelField(key.name+ ", " + 
                                       key.GetHashCode(), 
                stateManager.GetState(key)?.name);
        }         

    }
}