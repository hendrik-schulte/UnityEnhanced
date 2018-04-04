using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Instancing
{
    public interface IInstanciable
    {
        bool Instanced { get; }
        int InstanceCount { get; }
    }

    public abstract class InstanciableSO<T> : ScriptableObject, IInstanciable where T : InstanciableSO<T>
    {
        [SerializeField, HideInInspector] protected bool instanced;

        private Dictionary<Object, T> instances;

        public bool Instanced => instanced;
        public int InstanceCount => instances?.Count ?? 1;

//        protected virtual void Awake()
//        {
////            Logging.Log(this, "Awake");
//
////            Clear();
//        }

//        protected virtual void OnValidate()
//        {
//            if (!Application.isPlaying) Clear();
//        }

        /// <summary>
        /// Removes references to all instances of this ScriptableObject.
        /// </summary>
        public void Clear()
        {
            if (instanced)

                instances = new Dictionary<Object, T> {{this, (T) this}};
            else
                instances = null;
        }

        /// <summary>
        /// Returns an instance of this scriptable object based on the given key object.
        /// Returns the main object if it is null or instancing is not enabled. Should be
        /// used to access all instanced properties of this ScriptableObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Instance(Object key)
        {
            if (!instanced) return (T) this;
            if (key == null) return (T) this;

            if (instances == null) instances = new Dictionary<Object, T>();

            if (!instances.ContainsKey(key))
            {
                var instance = CreateInstance<T>();
                instance.name += "_" + key.GetInstanceID();
                instances.Add(key, instance);
            }

            return instances[key];
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(InstanciableSO<>), true)]
    [CanEditMultipleObjects]
    public class InstanciableSOEditor : Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty instanced;

        private void OnEnable()
        {
            m_Script = serializedObject.FindProperty("m_Script");
            instanced = serializedObject.FindProperty("instanced");
        }

        public override void OnInspectorGUI()
        {
            var instancedSO = target as IInstanciable;

            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.ObjectField(m_Script);
            GUI.enabled = true;

            const string tooltipInstancing =
                "When this is checked, this Object is automatically instaced at runtime. It then acts as a " +
                "template that can be reused. Scripts that utilize instancing need to inherit from " +
                "InstanceObserver.";

            instanced.boolValue = EditorGUILayout.Toggle(
                new GUIContent("Instanced", tooltipInstancing), instanced.boolValue);

            const string tooltipNo = "The number of instances currently referenced.";

            if (instancedSO.Instanced)
            {
                EditorGUILayout.LabelField(new GUIContent("# Instances", tooltipNo),
                    new GUIContent(instancedSO.InstanceCount.ToString()));
            }

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();

//            base.OnInspectorGUI();            
        }
    }
#endif
}