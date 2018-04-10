#if UNITY_EDITOR && UE_Photon
using UnityEditor;

namespace UE.PUNNetworking
{
    public static class ScriptableObjectEditorUtility
    {
        public static void PhotonControl(SerializedProperty PUNSync, SerializedProperty CachingOptions)
        {
            EditorGUILayout.PropertyField(PUNSync);

            if (!PUNSync.boolValue) return;
            
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(CachingOptions);
                
            EditorGUI.indentLevel--;
        }
    }
}
#endif