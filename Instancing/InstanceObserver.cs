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

        protected Object key => _key;

        public abstract IInstanciable GetTarget();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InstanceObserver), true)]
    [CanEditMultipleObjects]
    public class InstanceObserverEditor : Editor
    {
        private SerializedProperty key;

        private void OnEnable()
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
                            "This is the key to the ScriptableObject instance. Allows to reference " +
                            "a specific instance of the Object by hasing. This may be the root of a " +
                            "prefab to create an instance for every instance of the prefab."));
                }
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}