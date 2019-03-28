#if UNITY_EDITOR
using System.Linq;
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;

namespace UE.Instancing
{
    [CustomEditor(typeof(InstanciableSO<>), true)]
    [CanEditMultipleObjects]
    public abstract class InstanciableSOEditor : ReorderableArrayInspector
    {
        private SerializedProperty m_Script;
        private SerializedProperty instanced;

        protected override void InitInspector()
        {
            base.InitInspector();

            alwaysDrawInspector = true;

            m_Script = serializedObject.FindProperty("m_Script");
            instanced = serializedObject.FindProperty("instanced");
        }

        protected override void DrawInspector()
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

            if (instancedSO.Instanced)
            {
                EditorGUI.indentLevel++;

                var keys = instancedSO.Keys;

                if (keys != null)
                {
                    EditorGUILayout.Space();
                    DrawInstanceListHeader();

                    foreach (var key in keys)
                    {
                        if (key != null) DrawInstance(key);
                    }

                    EditorGUILayout.Space();
                }


                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();

            OnInspectorGUITop();

            serializedObject.Update();
            DrawPropertiesExcept(new[] {"m_Script"}.Concat(ExcludeProperties()).ToArray());
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

        /// <summary>
        /// Override this to draw editor controls underneath the instancing part and above the default inspector.
        /// </summary>
        protected virtual void OnInspectorGUITop()
        {
        }

        /// <summary>
        /// This draws the instance list header.
        /// </summary>
        protected virtual void DrawInstanceListHeader()
        {
            EditorGUILayout.LabelField("Name", "Key");
        }

        /// <summary>
        /// This is called for every instance and draws the list entry.
        /// </summary>
        /// <param name="key"></param>
        protected virtual void DrawInstance(Object key)
        {
            EditorGUILayout.LabelField(key.name, key.GetHashCode().ToString());
        }
    }
}
#endif