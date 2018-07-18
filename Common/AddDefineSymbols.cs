//This is adapted from https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Editor/AddDefineSymbols.cs

#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace UE.Common
{

    /// <summary>
    /// Adds the given define symbols to PlayerSettings define symbols.
    /// </summary>
    public class AddDefineSymbols : Editor
    {
        public static readonly string Photon = "UE_Photon";
        
        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        [DidReloadScripts]
        private static void CheckDependencies ()
        {
//            Logging.Log("AddDefineSymbols", "[Unity Enhanced] Adding Define Symbols.");
            
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
            var allDefines = definesString.Split ( ';' ).ToList ();

            if (allDefines.Contains(Photon))
            {
//                Debug.Log("Rmeoving photon directive.");
                allDefines.Remove(Photon);
            }
            
//            if(IsPhotonAvailable()) allDefines.Add(Photon);
            if(TypeExists("PhotonNetwork")) allDefines.Add(Photon);
            
//            Logging.Log(typeof(AddDefineSymbols), "photon available: " + TypeExists("PhotonNetwork"));
            
//            allDefines.AddRange ( Symbols.Except ( allDefines ) );
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup (
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join ( ";", allDefines.ToArray () ) );
        }

        private static bool IsPhotonAvailable() {
            var path = Application.dataPath + "/Plugins/Photon3Unity3D.dll";
            return File.Exists(path);
        }
        
        /// <summary>
        /// Checks for existance of the given type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static bool TypeExists(string typeName)
        {
            var foundType = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.Name == typeName
                select type).FirstOrDefault();

            return foundType != null;
        }
    }
}
#endif