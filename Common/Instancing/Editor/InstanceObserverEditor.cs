#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UE.Common.SubjectNerd.Utilities;
#if UE_Photon
using UE.PUNNetworking;
#if PUN_2_OR_NEWER
using Photon.Pun;
#endif
#endif
using UnityEditor;
using UnityEngine;

namespace UE.Instancing
{
    [CustomEditor(typeof(InstanceObserver), true)]
    [CanEditMultipleObjects]
    public class InstanceObserverEditor : ReorderableArrayInspector
    {
        private SerializedProperty key;

        protected override void InitInspector()
        {
            base.InitInspector();

            alwaysDrawInspector = true;

            key = serializedObject.FindProperty("_key");
        }

        protected override void DrawInspector()
        {
            var listener = target as InstanceObserver;
            serializedObject.Update();

            if (listener.Target != null)
            {
                if (listener.Target.Instanced)
                {
                    if (key.objectReferenceValue == null)
                        EditorGUILayout.HelpBox(
                            InstanceReference.WARNING_INSTANCE_KEY_EMPTY,
                            MessageType.Error);

#if UE_Photon
                    else if (listener.Target.PhotonSyncSettings.PUNSync)
                    {
                        var keyGO = (key.objectReferenceValue as GameObject)?.GetPhotonView();

                        if (keyGO == null) (key.objectReferenceValue as Component)?.GetComponent<PhotonView>();
                        
                        if (keyGO == null) EditorGUILayout.HelpBox(
                            PhotonSync.WARNING_INSTANCE_KEY_NO_PHOTONVIEW, 
                            MessageType.Error);                            
                    }
#endif

                    EditorGUILayout.ObjectField(
                        key, new GUIContent("Instance Key",
                            "This is the key to an instance of the ScriptableObject below. Allows to reference " +
                            "a specific instance of the Object by hashing the key. This may be the root of a " +
                            "prefab to create an instance for every instance of the prefab."));
                }
            }

            DrawPropertiesExcept(new[] {"m_Script"}.Concat(ExcludeProperties()).ToArray());

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// This can be overridden to exclude the given properties from being displayed in the inspector.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<string> ExcludeProperties()
        {
            return new string[0];
        }
    }
}
#endif