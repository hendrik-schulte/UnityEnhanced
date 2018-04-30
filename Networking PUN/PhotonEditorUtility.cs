#if UE_Photon
#if UNITY_EDITOR
using System;
using UnityEditor;
#endif

namespace UE.PUNNetworking
{
    [Obsolete]
    public static class PhotonEditorUtility
    {
        #if UNITY_EDITOR
        public static void PhotonControl(SerializedProperty PUNSync, SerializedProperty CachingOptions)
        {
            EditorGUILayout.PropertyField(PUNSync);

            if (!PUNSync.boolValue) return;
            
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(CachingOptions);
                
            EditorGUI.indentLevel--;
        }
        #endif
    }
}
#endif