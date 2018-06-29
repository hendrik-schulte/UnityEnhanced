using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
#if UE_Photon
using UE.PUNNetworking;
#endif
using UE.StateMachine;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Instancing
{
    /// <inheritdoc />
    /// <summary>
    /// This class can be inherited to utilize the ScriptableObject instancing feature.
    /// By defining a key object, you access a specific instance of the referenced
    /// InstanciableSO. The key is used within the entire class. Is is recommended to
    /// use the respective InstancedX wrappers (e.g. <see cref="InstancedState"/>) to
    /// access objects instead of this class, to be more flexible.
    /// </summary>
    public abstract class InstanceObserver : MonoBehaviour, IInstanceReference
    {
        [SerializeField, HideInInspector] private Object _key;

        public virtual Object Key
        {
            get { return _key; }
            set
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Logging.Warning(this, "Setting instance key at runtime. This is not recommended " +
                                          "and might cause undefined behaviour.");
#endif
                _key = value;
            }
        }

        /// <summary>
        /// Returns the instanciated object. It is used to display the instance key property in the inspector
        /// only when instancing is enabled for the returned object.
        /// </summary>
        /// <returns></returns>
        public abstract IInstanciable Target { get; }

#if UNITY_EDITOR && UE_Photon
        public bool HasValidNetworkingKey => PhotonSync.ValidNetworkingKey(Target, Key);
#endif
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
                            PhotonSync.WARNING_INSTANCE_KEY_WRONG, 
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
#endif
}