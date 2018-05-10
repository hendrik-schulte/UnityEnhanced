using System;

#if UE_Photon

namespace UE.PUNNetworking
{
    /// <summary>
    /// This interface is implemented by ScriptableObjects that are synchronized by PhotonSyncManager.
    /// </summary>
    [Obsolete]
    public interface ISyncable
    {
        bool PUNSyncEnabled { get; }
        
        EventCaching CachingOptions { get; }

        /// <summary>
        /// When this is true, events are not broadcasted. Used to avoid echoing effects.
        /// </summary>
        bool MuteNetworkBroadcasting { get; set; }
    }
}
#endif