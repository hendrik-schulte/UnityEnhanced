#if UE_Photon
using System;
using System.Collections.Generic;
using UE.Common;
using UE.Events;
using UE.StateMachine;
using UnityEngine;

#if PUN_2_OR_NEWER
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
#else
using Photon;
#endif

namespace UE.PUNNetworking
{
    /// <summary>
    /// This component is used to synchronize UE.Events and UE.StateMachine automatically
    /// within a Photon Networking system.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Networking Photon/PhotonSyncManager")]
    public class PhotonSyncManager : 
#if PUN_2_OR_NEWER
        MonoBehaviourPunCallbacks, IOnEventCallback
#else
        PunBehaviour
#endif
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
        private const byte EventRaiseUEQuaternionEvent = 110;

        #region Resources Subfolder
        
        private const string EventSubfolder = "Events";
        private const string StateMachineSubfolder = "States";
        
        /// <summary>
        /// Returns the allowed resources subfolders of the given types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetResourcesSubfolder(Type type)
        {
            if (type == typeof(StateManager))
                return StateMachineSubfolder;
            if (type == typeof(GameEvent)
                || type.IsSubClassOfGeneric(typeof(ParameterEvent<,>))
            )
                return EventSubfolder;

            return "";
        }
        
        #endregion

        #region StateMachineCache

        private static List<StateManager> syncedStateManager;

        /// <summary>
        /// Registeres the given StateMachine so that it is synced towards new players.
        /// </summary>
        /// <param name="stateManager"></param>
        public static void RegisterStateManager(StateManager stateManager)
        {
            if (syncedStateManager == null)
                syncedStateManager = new List<StateManager>();

            if (syncedStateManager.Contains(stateManager))
                return;

            syncedStateManager.Add(stateManager);
        }

        /// <inheritdoc />
        /// <summary>
        /// When a new player connects, send him the current state.
        /// </summary>
        /// <param name="newPlayer"></param>
#if PUN_2_OR_NEWER
        public override void OnPlayerEnteredRoom(Player newPlayer)  
        {
            if (!PhotonNetwork.IsMasterClient) return;
#else
        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)  
            {
            if (!PhotonNetwork.isMasterClient) return;
#endif

            if (syncedStateManager == null)
            {
                if (debugLog) Logging.Warning(this, "syncedStateManager is null.");
                return;
            }

            if (debugLog) Logging.Log(this, "Master Client: Propagating current states to new players.");

            foreach (var sm in syncedStateManager)
            {
                sm.PropagateStatePhoton();
            }
        }

        #endregion

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
            if (!syncable.PUNSyncEnabled || !InRoom() || syncable.MuteNetworkBroadcasting) return;

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

            RaiseEvent(eventCode, content, raiseEventOptions);
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
            if (!settings.PUNSync || !InRoom() || settings.MuteNetworkBroadcasting) return;

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

            RaiseEvent(eventCode, content, raiseEventOptions);
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
            if (!syncable.PUNSyncEnabled || !InRoom() || syncable.MuteNetworkBroadcasting) return;

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

            RaiseEvent(TypeToCode(typeof(T)), content, raiseEventOptions);
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
            if (!settings.PUNSync || !InRoom() || settings.MuteNetworkBroadcasting) return;

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

            RaiseEvent(TypeToCode(typeof(T)), content, raiseEventOptions);
        }

        /// <summary>
        /// API-neutral call to PhotonNetwork.RaiseEvent.
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="content"></param>
        /// <param name="raiseEventOptions"></param>
        private static void RaiseEvent(byte eventCode, object content, RaiseEventOptions raiseEventOptions)
        {
#if PUN_2_OR_NEWER
            PhotonNetwork.RaiseEvent(eventCode, content, raiseEventOptions, SendOptions.SendReliable);
#else
            PhotonNetwork.RaiseEvent(eventCode, content, true, raiseEventOptions);
#endif
        }

        /// <summary>
        /// API-neutral call to PhotonNetwork.InRoom
        /// </summary>
        /// <returns></returns>
        private static bool InRoom()
        {
#if PUN_2_OR_NEWER
            return PhotonNetwork.InRoom;
#else
            return PhotonNetwork.inRoom;
#endif
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

            if (type == typeof(Quaternion))
                return EventRaiseUEQuaternionEvent;

            Logging.Error("Photon Sync", "Trying to send a parameter event that cannot be serialized.");
            return 0;
        }

        #endregion

        #region Receiving

#if PUN_2_OR_NEWER
        /// <summary>
        /// Receives a photon event in PUN 2.
        /// </summary>
        /// <param name="photonEvent"></param>
        public void OnEvent(EventData photonEvent)
        {
            OnEvent(photonEvent.Code, photonEvent.CustomData, photonEvent.Sender);
        }
#else
        void OnEnable()
        {
            PhotonNetwork.OnEventCall += OnEvent;
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= OnEvent;
        }
#endif
    
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

                case EventRaiseUEQuaternionEvent:

                    RemoteParameterEvent<Quaternion, QuaternionEvent>(
                        contentAr[0] as string, ParseID(contentAr), (Quaternion) contentAr[2]);
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

            if (!success) gameEvent = CachedResources.Load<GameEvent>(EventSubfolder + "/" + eventName, out success);

            if (!success)
            {
                Logging.Error(this, eventName + " could not be found in a resources folder!");
                return;
            }

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

            if (!success) paramEvent = CachedResources.Load<TS>(EventSubfolder + "/" + eventName, out success);

            if (!success)
            {
                Logging.Error(this, eventName + " could not be found in a resources folder!");
                return;
            }

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
            Logging.Log(this, stateName + " instanced state enter event received! key: " + key, debugLog);

            bool success;
            var state = CachedResources.Load<State>(stateName, out success);

            if (!success) state = CachedResources.Load<State>(StateMachineSubfolder + "/" + stateName, out success);

            if (!success)
            {
                Logging.Error(this, stateName + " could not be found in a resources folder!");
                return;
            }

            state.stateManager.MuteNetworkBroadcasting = true;
            if (key == -1) state.Enter(); //non-instanced
            else state.Enter(state.stateManager.GetByKeyId(key)); //instanced. Getting the right key by network key
            state.stateManager.MuteNetworkBroadcasting = false;
        }

        #endregion
    }
}
#endif