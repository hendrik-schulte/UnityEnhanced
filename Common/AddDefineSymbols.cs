//This is adapted from https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Editor/AddDefineSymbols.cs

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UE.Common
{

    /// <summary>
    /// Adds the given define symbols to PlayerSettings define symbols.
    /// Just add your own define symbols to the Symbols property at the below.
    /// </summary>
    [InitializeOnLoad]
    public class AddDefineSymbols : Editor
    {
//        /// <summary>
//        /// Symbols that will be added to the editor
//        /// </summary>
//        public static readonly string [] Symbols = new string[] {
//            "MYCOMPANY",
//            "MYCOMPANY_MYPACKAGE"
//        };

        public static readonly string Photon = "UE_Photon";
        
        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        static AddDefineSymbols ()
        {
            Debug.Log("[Unity Enhanced]: Adding Define Symbols.");
            
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup ( EditorUserBuildSettings.selectedBuildTargetGroup );
            List<string> allDefines = definesString.Split ( ';' ).ToList ();

            if (allDefines.Contains(Photon))
            {
//                Debug.Log("Rmeoving photon directive.");
                allDefines.Remove(Photon);
            }
            
            if(IsPhotonAvailable()) allDefines.Add(Photon);
            
//            allDefines.AddRange ( Symbols.Except ( allDefines ) );
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup (
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join ( ";", allDefines.ToArray () ) );
        }

        private static bool IsPhotonAvailable() {
            string path = Application.dataPath + "/Plugins/Photon3Unity3D.dll";
            return File.Exists(path);
        }
    }
}
#endif