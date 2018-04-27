using UE.Common;
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
        /// The instance key defined in the inspector.
        /// </summary>
        protected Object key => _key;

        /// <summary>
        /// Returns the instanciated object.
        /// </summary>
        /// <returns></returns>
        public abstract IInstanciable GetTarget();

        /// <summary>
        /// Sets the instance key.
        /// </summary>
        /// <param name="instanceKey"></param>
        public virtual void SetKey(Object instanceKey)
        {
            if(Application.isPlaying) 
                Logging.Warning(this, "Setting instance key at runtime. This is not recommended " +
                                      "and might cause undefined behaviour.");
            
            _key = instanceKey;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InstanceObserver), true)]
    [CanEditMultipleObjects]
    public class InstanceObserverEditor : Editor
    {
        private SerializedProperty key;

        protected virtual void OnEnable()
        {
            key = serializedObject.FindProperty("_key");
        }

        public override void OnInspectorGUI()
        {
            var listener = target as InstanceObserver;
            serializedObject.Update();

            if (listener.GetTarget() != null)
            {
                if (listener.GetTarget().Instanced)
                {
                    EditorGUILayout.ObjectField(
                        key, new GUIContent("Instance Key",
                            "This is the key to an instance of the ScriptableObject below. Allows to reference " +
                            "a specific instance of the Object by hashing the key. This may be the root of a " +
                            "prefab to create an instance for every instance of the prefab."));
                }
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}