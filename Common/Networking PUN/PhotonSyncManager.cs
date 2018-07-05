#if UE_Photon

using System;
using System.Collections.Generic;
using Photon;
using UE.Common;
using UE.Events;
using UE.StateMachine;
using UnityEngine;

namespace UE.PUNNetworking
{
    /// <summary>
    /// This component is used to synchronize UE.Events and UE.StateMachine automatically
    /// within a Photon Networking system.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Networking Photon/PhotonSyncManager")]
    public class PhotonSyncManager : PunBehaviour
    {
        [SerializeField] private bool debugLog;

        public const byte EventStateChange = 103;
        public const byte EventRaiseUEGameEvent = 102;
        private const byte EventRaiseUEFloatEvent = 104;
        private const byte EventRaiseUEStringEvent = 105;
        private const byte EventRaiseUEBoolEvent = 106;
        private const byte EventRaiseUEIntEvent = 107;
        private const byte EventRaiseUEVector2Event = 108;
        private const byte EventRaiseUEVector3Event = 109;

        #region StateMachineCache

        private static List<StateManager> syncedStateManager;

        public static void RegisterStateManager(StateManager stateManager)
        {
            if (syncedStateManager == null)
                syncedStateManager = new List<StateManager>();

            if (syncedStateManager.Contains(stateManager))
            {
//                Logging.Warning(typeof(PhotonSyncManager), "The state manager registered does already exist.");
                return;
            }
            
//            Logging.Log(typeof(PhotonSyncManager), "Adding " + stateManager.name);
            syncedStateManager.Add(stateManager);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (!PhotonNetwork.isMasterClient) return;

            if(syncedStateManager == null) return;
            
            foreach (var sm in syncedStateManager)
            {
                sm.PropagateStatePhoton();
//                SendEvent(sm.PhotonSyncSettings, EventStateChange, sm.GetState(), sm.KeyID);
            }
        }

        #endregion

        void OnEnable()
        {
            PhotonNetwork.OnEventCall += OnEvent;
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= OnEvent;
        }

        #region Sending

        /// <summary>
        /// Send an event from an ISyncable Object.
        /// </summary>
        /// <param name="syncable">This is used to access parameters of the sender.</param>
        /// <param name="eventCode"></param>
        /// <param name="name">The name of the ScriptableObject. It is used to Load the Resource at the remote.</param>
        /// <param name="keyID">This is used to identify instanced Objects.</param>
        [Obsolete]
        public static void SendEvent(ISyncable syncable, byte eventCode, string name, int keyID)
        {
            if (!syncable.PUNSyncEnabled || !PhotonNetwork.inRoom || syncable.MuteNetworkBroadcasting) return;

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
        
        /// <summary>
        /// Sends an event with the given sync settings.
        /// </summary>
        /// <param name="settings">This defines sync settings</param>
        /// <param name="eventCode"></param>
        /// <param name="name">The name of the ScriptableObject. It is used to Load the Resource at the remote.</param>
        /// <param name="keyID">This is used to identify instanced Objects.</param>
        public static void SendEvent(PhotonSync settings, byte eventCode, string name, int keyID)
        {
            if (!settings.PUNSync || !PhotonNetwork.inRoom || settings.MuteNetworkBroadcasting) return;

            var raiseEventOptions = new RaiseEventOptions()
            {
                CachingOption = settings.cachingOptions,
                Receivers = ReceiverGroup.Others
            };

            var content = new object[]
            {
                name,
                keyID,
            };

            PhotonNetwork.RaiseEvent(eventCode, content, true, raiseEventOptions);
        }

        /// <summary>
        /// Send an event from an ISyncable Object.
        /// </summary>
        /// <param name="syncable">This is used to access parameters of the sender.</param>
        /// <param name="eventCode"></param>
        /// <param name="name">The name of the ScriptableObject. It is used to Load the Resource at the remote.</param>
        /// <param name="keyID">This is used to identify instanced Objects.</param>
        [Obsolete]
        public static void SendEventParam<T>(ISyncable syncable, string name, int keyID, T parameter)
        {
            if (!syncable.PUNSyncEnabled || !PhotonNetwork.inRoom || syncable.MuteNetworkBroadcasting) return;

            var raiseEventOptions = new RaiseEventOptions()
            {
                CachingOption = syncable.CachingOptions,
                Receivers = ReceiverGroup.Others
            };

            var content = new object[]
            {
                name,
                keyID,
                parameter
            };

            PhotonNetwork.RaiseEvent(TypeToCode(typeof(T)), content, true, raiseEventOptions);
        }
        
        /// <summary>
        /// Sends a parameter event with the given sync settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <param name="keyID"></param>
        /// <param name="parameter"></param>
        /// <typeparam name="T"></typeparam>
        public static void SendEventParam<T>(PhotonSync settings, string name, int keyID, T parameter)
        {
            if (!settings.PUNSync|| !PhotonNetwork.inRoom || settings.MuteNetworkBroadcasting) return;

            var raiseEventOptions = new RaiseEventOptions()
            {
                CachingOption = settings.cachingOptions,
                Receivers = ReceiverGroup.Others
            };

            var content = new object[]
            {
                name,
                keyID,
                parameter
            };

            PhotonNetwork.RaiseEvent(TypeToCode(typeof(T)), content, true, raiseEventOptions);
        }

        /// <summary>
        /// Given a type this returns the corresponding eventCode.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static byte TypeToCode(Type type)
        {
            if (type == typeof(float))
                return EventRaiseUEFloatEvent;

            if (type == typeof(string))
                return EventRaiseUEStringEvent;

            if (type == typeof(bool))
                return EventRaiseUEBoolEvent;

            if (type == typeof(int))
                return EventRaiseUEIntEvent;

            if (type == typeof(Vector2))
                return EventRaiseUEVector2Event;

            if (type == typeof(Vector3))
                return EventRaiseUEVector3Event;

            Logging.Error("Phyton Sync", "Trying to send a parameter event that cannot be serialized.");
            return 0;
        }

        #endregion

        #region Receiving

        /// <summary>
        /// Receives a photon event and calls the corresponding remote function.
        /// </summary>
        /// <param name="eventcode"></param>
        /// <param name="content"></param>
        /// <param name="senderid"></param>
        void OnEvent(byte eventcode, object content, int senderid)
        {
            var contentAr = content as object[];
            
            switch (eventcode)
            {
                case EventStateChange:

                    RemoteEnterState(contentAr[0] as string, ParseID(contentAr));
                    break;

                case EventRaiseUEGameEvent:

                    RemoteGameEvent(contentAr[0] as string, ParseID(contentAr));
                    break;

                case EventRaiseUEFloatEvent:

                    RemoteParameterEvent<float, FloatEvent>(
                        contentAr[0] as string, ParseID(contentAr), (float) contentAr[2]);
                    break;

                case EventRaiseUEStringEvent:

                    RemoteParameterEvent<string, StringEvent>(
                        contentAr[0] as string, ParseID(contentAr), (string) contentAr[2]);
                    break;

                case EventRaiseUEBoolEvent:

                    RemoteParameterEvent<bool, BoolEvent>(
                        contentAr[0] as string, ParseID(contentAr), (bool) contentAr[2]);
                    break;

                case EventRaiseUEIntEvent:

                    RemoteParameterEvent<int, IntEvent>(
                        contentAr[0] as string, ParseID(contentAr), (int) contentAr[2]);
                    break;

                case EventRaiseUEVector2Event:

                    RemoteParameterEvent<Vector3, Vector3Event>(
                        contentAr[0] as string, ParseID(contentAr), (Vector2) contentAr[2]);
                    break;

                case EventRaiseUEVector3Event:

                    RemoteParameterEvent<Vector3, Vector3Event>(
                        contentAr[0] as string, ParseID(contentAr), (Vector3) contentAr[2]);
                    break;
            }
        }

        /// <summary>
        /// Parses the instance id from the object array. If its null, set it to -1 (non-instanced).
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static int ParseID(object[] content)
        {
            if (content[1] == null) return -1;
            return (int) content[1];
        }

        /// <summary>
        /// This is called on the remote when a Events.GameEvent is fired.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="key"></param>
        private void RemoteGameEvent(string eventName, int key)
        {
            Logging.Log(this, eventName + " GameEvent received! key: " + key, debugLog);

            bool success;
            var gameEvent = CachedResources.Load<GameEvent>(eventName, out success);
            if (!success) return;

            gameEvent.MuteNetworkBroadcasting = true;
            if (key == -1) gameEvent.Raise(); //non-instanced
            else gameEvent.Raise(gameEvent.GetByKeyId(key)); //instanced. Getting the right key by network key
            gameEvent.MuteNetworkBroadcasting = false;
        }

        /// <summary>
        /// This is called on the remote when a Events.ParameterEvent is fired.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="key"></param>
        private void RemoteParameterEvent<T, TS>(string eventName, int key, T value) where TS : ParameterEvent<T, TS>
        {
            Logging.Log(this, eventName + " " + typeof(TS) + " received! key: " + key, debugLog);

            bool success;
            var paramEvent = CachedResources.Load<TS>(eventName, out success);
            if (!success) return;

            paramEvent.MuteNetworkBroadcasting = true;
            if (key == -1) paramEvent.Raise(value); //non-instanced
            else paramEvent.Raise(value, paramEvent.GetByKeyId(key)); //instanced. Getting the right key by network key
            paramEvent.MuteNetworkBroadcasting = false;
        }

        /// <summary>
        /// This is called on the remote when a state is entered.
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="key"></param>
        private void RemoteEnterState(string stateName, int key)
        {
            Logging.Log(this, stateName + " instanced Enter event received!", debugLog);

            bool success;
            var state = CachedResources.Load<State>(stateName, out success);
            if (!success) return;

            state.stateManager.MuteNetworkBroadcasting = true;
            if (key == -1) state.Enter(); //non-instanced
            else state.Enter(state.stateManager.GetByKeyId(key)); //instanced. Getting the right key by network key
            state.stateManager.MuteNetworkBroadcasting = false;
        }

        #endregion
    }
}
#endif