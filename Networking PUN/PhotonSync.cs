#if UE_Photon

using System.Collections.Generic;
using Photon;
using UE.Common;
using UE.Events;
using UE.StateMachine;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UE.PUNNetworking
{
    /// <summary>
    /// This component is used to synchronize UE.Events and UE.StateMachine automatically
    /// within a Photon Networking system.
    /// </summary>
    public class PhotonSync : PunBehaviour
    {
        [SerializeField] private bool debugLog;

        private readonly Dictionary<string, GameEvent> eventCache = new Dictionary<string, GameEvent>();
        private readonly Dictionary<string, State> stateCache = new Dictionary<string, State>();

        public const byte EventStateChange = 103;
        public const byte EventRaiseUEGameEvent = 102;

        void OnEnable()
        {
            PhotonNetwork.OnEventCall += OnEvent;
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= OnEvent;
        }

        public static void SendEvent(ISynchable syncable, byte eventCode, string name, int keyID)
        {
            if (syncable.PUNSyncEnabled && PhotonNetwork.inRoom && !syncable.MuteNetworkBroadcasting )
            {
                var raiseEventOptions = new RaiseEventOptions()
                {
                    CachingOption = syncable.CachingOptions,
                    Receivers = ReceiverGroup.Others
                };

                var content = new object[]
                {
                    name,
                    keyID,
                };

                PhotonNetwork.RaiseEvent(eventCode, content, true, raiseEventOptions);
            }
        }

        #region Receiving

        void OnEvent(byte eventcode, object content, int senderid)
        {
            switch (eventcode)
            {
                case EventStateChange:

                    var contentAr = content as object[];

                    if (contentAr[1] == null) RemoteEnterState(contentAr[0] as string, -1);
                    else RemoteEnterState(contentAr[0] as string, (int) contentAr[1]);
                    break;

                case EventRaiseUEGameEvent:

                    contentAr = content as object[];
                    if (contentAr[1] == null) RemoteEvent(contentAr[0] as string, -1);
                    else RemoteEvent(contentAr[0] as string, (int) contentAr[1]);
                    break;
            }
        }

        private void RemoteEvent(string eventName, int key)
        {
            Logging.Log(this, eventName + " instanced GameEvent received!", debugLog);

            bool success;
            var gameEvent = Load(eventCache, eventName, out success);
            if (!success) return;
            
            gameEvent.MuteNetworkBroadcasting = true;
            if(key == -1) gameEvent.Raise();
            else gameEvent.Raise(gameEvent.GetByKeyId(key));
            gameEvent.MuteNetworkBroadcasting = false;
        }

        private void RemoteEnterState(string stateName, int key)
        {
            Logging.Log(this, stateName + " instanced Enter event received!", debugLog);

            bool success;
            var state = Load(stateCache, stateName, out success);
            if (!success) return;

            state.stateManager.MuteNetworkBroadcasting = true;
            if(key == -1) state.Enter();
            else state.Enter(state.stateManager.GetByKeyId(key));
            state.stateManager.MuteNetworkBroadcasting = false;
        }

        #endregion

        /// <summary>
        /// Attempts to find the asset in the cache. When not found,
        /// it tries to load it from a Resources folder and caches it.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="success"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(IDictionary<string, T> cache, string key, out bool success) where T : Object
        {
            T asset;

            if (cache.ContainsKey(key))
            {
//            Logging.Log("Photon Sync", "Loading asset from dictionary.");
                asset = cache[key];
            }
            else
            {
//            Logging.Log("Photon Sync", "Can't find the asset in the cache. Calling Resources.Load ...");
                asset = Resources.Load<T>(key);
                cache.Add(key, asset);
            }

            if (!asset)
            {
                Logging.Error("Photon Sync", "The desired asset named '" + key + "' does not exist.");
                success = false;
                return null;
            }
            else
            {
                success = true;
                return asset;
            }
        }
    }
}

#endif