using System.Linq;
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Instancing
{
    /// <summary>
    /// This class can be inherited to utilize the ScriptableObject instancing feature.
    /// By defining a key object, you access a specific instance of the referenced
    /// InstanciableSO.
    /// </summary>
    public abstract class InstanceObserver : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Object _key;

        /// <summary>
        /// The instance key defined in the inspector. Use this for all calls on the instanciated object.
        /// </summary>
        protected Object key => _key;

        /// <summary>
        /// Returns the instanciated object. It is used to display the instance key property in the inspector
        /// only when instancing is enabled for the returned object.
        /// </summary>
        /// <returns></returns>
        public abstract IInstanciable GetTarget();

        /// <summary>
        /// Sets the instance key.
        /// </summary>
        /// <param name="instanceKey"></param>
        public virtual void SetKey(Object instanceKey)
        {
            if (Application.isPlaying)
                Logging.Warning(this, "Setting instance key at runtime. This is not recommended " +
                                      "and might cause undefined behaviour.");

            _key = instanceKey;
        }
    }

#if UNITY_EDITOR
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

            if (listener.GetTarget() != null)
            {
                if (listener.GetTarget().Instanced)
                {
                    if (key.objectReferenceValue == null) EditorGUILayout.HelpBox(
                        "Your target asset has instancing enabled but you did not assign an Instance Key.", 
                        MessageType.Error);    
                    
#if UE_Photon
                    else if (listener.GetTarget().PhotonSyncSettings.PUNSync)
                    {
                        PhotonView keyGO;

                        keyGO = (key.objectReferenceValue as GameObject)?.GetPhotonView();

                        if (keyGO == null) (key.objectReferenceValue as Component)?.GetComponent<PhotonView>();
                        
                        if (keyGO == null) EditorGUILayout.HelpBox(
                            "Photon Sync is enabled for your target asset, but your Instance Key Object has no " +
                            "PhotonView attached. You need to assign a PhotonView component or a parenting " +
                            "GameObject!", 
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

            DrawPropertiesExcept(new[]{"m_Script"}.Concat(ExcludeProperties()).ToArray());

            serializedObject.ApplyModifiedProperties();
        }
        
        /// <summary>
        /// This can be overridden to exclude the given properties from being displayed in the inspector.
        /// </summary>
        /// <returns></returns>
        protected virtual string[] ExcludeProperties()
        {
            return new string[0];
        }
    }
#endif
}