//This is adapted from https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Editor/AddDefineSymbols.cs

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private static string Photon => "UE_Photon";
        private static string SharpConfig => "UE_SharpConfig";

        /// <summary>
        /// Add define symbols as soon as Unity gets done compiling.
        /// </summary>
        [DidReloadScripts]
        private static void CheckDependencies()
        {
            var definesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var allDefines = definesString.Split(';').ToList();

            ManageDefine(ref allDefines, Photon, TypeExists("PhotonNetwork"));
            ManageDefine(ref allDefines, SharpConfig, NamespaceExists("SharpConfig"));
            
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }

        private static void Check(string ns)
        {
            Logging.Log(typeof(AddDefineSymbols), "Checking for '" + ns + "' " + NamespaceExists(ns));
        }

        /// <summary>
        /// Adds or removes the given define symbol.
        /// </summary>
        /// <param name="allDefines"></param>
        /// <param name="defineName"></param>
        /// <param name="exists"></param>
        private static void ManageDefine(ref List<string> allDefines, string defineName, bool exists)
        {
            var alreadyDefined = allDefines.Contains(defineName);

            if (exists && !alreadyDefined)
                allDefines.Add(defineName);
                    
            if (!exists && alreadyDefined)
                allDefines.Remove(defineName);
        }

        private static bool IsPhotonAvailable()
        {
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
        
        /// <summary>
        /// Checks for existance of the given namespace.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        public static bool NamespaceExists(string namespaceName)
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.Namespace == namespaceName
                select type).Any();
        }
    }
}
#endif