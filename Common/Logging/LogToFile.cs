using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Common
{
    /// <summary>
    /// This wraps all settings for file logging.
    /// </summary>
    [Serializable]
    public class LogToFile
    {
        [Tooltip("Enables automatic logging to a file.")]
        public bool logToFile;

        [Tooltip("Name of the log file.")] public string FileName = "main.log";

//        [Tooltip("When logToFile, a separate log file is created for every instance.")]
//        public bool UniqueForEachInstance = true;

//        [SerializeField] internal bool Instanciable;
//
//        public LogToFile(Object parent = null)
//        {
//            if (parent == null)
//                Instanciable = false;
//            else
//                Instanciable = parent is IInstanciable;
//        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(LogToFile))]
    public class LogToFileDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabled = property.FindPropertyRelative("logToFile");

            EditorGUI.PropertyField(position.GetLine(1), enabled);

            if (enabled.boolValue)
            {
                EditorGUI.indentLevel++;

                EditorGUI.PropertyField(
                    position.GetLine(2),
                    property.FindPropertyRelative("FileName"));

//                EditorGUI.PropertyField(
//                    GetSubRect(position, LineHeight * 2, LineHeight),
//                    property.FindPropertyRelative("UniqueForEachInstance"));

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 1;

            if (property.FindPropertyRelative("logToFile").boolValue)
                lines++;
                
            return EditorUtil.PropertyHeight(lines);
        }

//        private bool IsInstanciable(SerializedProperty property)
//        {
//            return property.FindPropertyRelative("Instaciable").boolValue;
//        }
    }
#endif
}