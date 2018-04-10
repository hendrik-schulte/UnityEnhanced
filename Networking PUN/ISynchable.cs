#if UE_Photon

namespace UE.PUNNetworking
{
    public interface ISynchable
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