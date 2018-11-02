#if UE_Photon
using System;
using System.Linq;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using Object = UnityEngine.Object;
#if PUN_2_OR_NEWER
using Photon.Realtime;
using Photon.Pun;
#endif

namespace UE.PUNNetworking
{
    /// <summary>
    /// This wrapps some settings of the Photon sync adapter.
    /// </summary>
    [Serializable]
    public struct PhotonSync
    {
        [Tooltip("Enables automatic sync in a Photon network. " +
                 "You need to have a PhotonSyncManager Component in your Scene and " +
                 "this asset needs to be located at the root of a Resources folder " +
                 "with a unique name.")]
        public bool PUNSync;

        [Tooltip("How should the event be cached by Photon?")]
        public EventCaching cachingOptions;

        [NonSerialized] public bool MuteNetworkBroadcasting;

#if UNITY_EDITOR
        public static string WARNING_INSTANCE_KEY_NO_PHOTONVIEW => 
            "Photon Sync is enabled for your target asset, but your Instance Key Object has no " +
            "PhotonView attached. You need to assign a PhotonView component or a parenting GameObject!";
        
//        public static string WARNING_ASSET_NOT_IN_RESOURCES_FOLDER => 
//            "Photon Sync is enabled for this asset, but it is not located at the root of a " +
//            "resources folder! This is required for sync to work.";
        
        public static string WARNING_ASSET_NOT_IN_RESOURCES_FOLDER(string subfolder)
        {
            if (subfolder.Any())
                return "Photon Sync is enabled for this asset, but it is not located at the root of a " +
                       "resources folder (or Resources/" + subfolder + "/)! This is required for sync to work.";
            
            return "Photon Sync is enabled for this asset, but it is not located at the root of a " +
                   "resources folder! This is required for sync to work.";
        }

        /// <summary>
        /// Returns true when the given key meets the requirements for networking.
        /// </summary>
        /// <param name="instanciable"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ValidNetworkingKey(IInstanciable instanciable, Object key)
        {
            if (!instanciable.PhotonSyncSettings.PUNSync) return true;

            var keyGO = (key as GameObject)?.GetPhotonView();

            if (keyGO == null) (key as Component)?.GetComponent<PhotonView>();

            return keyGO != null;
        }
#endif
    }
}
#endif