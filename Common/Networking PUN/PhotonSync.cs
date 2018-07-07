#if UE_Photon
using System;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using Object = UnityEngine.Object;

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
        public static string WARNING_INSTANCE_KEY_WRONG = 
            "Photon Sync is enabled for your target asset, but your Instance Key Object has no " +
            "PhotonView attached. You need to assign a PhotonView component or a parenting GameObject!";

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