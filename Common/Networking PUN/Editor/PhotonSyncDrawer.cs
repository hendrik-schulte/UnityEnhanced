#if UNITY_EDITOR && UE_Photon
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEngine;

namespace UE.PUNNetworking
{
    [CustomPropertyDrawer(typeof(PhotonSync))]
    public class PhotonSyncDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var enabled = property.FindPropertyRelative("PUNSync");

            EditorGUI.PropertyField(position.GetLine(1), enabled);

            if (enabled.boolValue)
            {
                EditorGUI.indentLevel++;

                var line = 2;
                
                DrawResourcesWarning(position, property, ref line);
                
                EditorGUI.PropertyField(
                    position.GetLine(line),
                    property.FindPropertyRelative("cachingOptions"));

                EditorGUI.indentLevel--;
                
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 1;

            if (property.FindPropertyRelative("PUNSync").boolValue)
            {
                lines++;

                if (!property.IsInResourcesFolder(PhotonSyncManager.GetResourcesSubfolder(property.GetParent().GetType())))
                    lines += 2;
            }
            
            return EditorUtil.PropertyHeight(lines);
        }

        private static void DrawResourcesWarning(Rect position, SerializedProperty property, ref int line)
        {
            var subfolder = PhotonSyncManager.GetResourcesSubfolder(property.GetParent().GetType());
            
            if(property.IsInResourcesFolder(subfolder)) return;

            EditorGUI.HelpBox(position.GetLines(line, 2), PhotonSync.WARNING_ASSET_NOT_IN_RESOURCES_FOLDER(subfolder), MessageType.Error);
            line += 2;
        }
    }
}
#endif