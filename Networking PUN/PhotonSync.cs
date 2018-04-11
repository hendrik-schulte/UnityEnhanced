#if UE_Photon

using System;
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

        /// <summary>
        /// This is the Resources cache.
        /// </summary>
        private readonly Dictionary<string, Object> cache = new Dictionary<string, Object>();

        public const byte EventStateChange = 103;
        public const byte EventRaiseUEGameEvent = 102;
        private const byte EventRaiseUEFloatEvent = 104;
        private const byte EventRaiseUEStringEvent = 105;
        private const byte EventRaiseUEBoolEvent = 106;
        private const byte EventRaiseUEIntEvent = 107;
        private const byte EventRaiseUEVector2Event = 108;
        private const byte EventRaiseUEVector3Event = 109;

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
        /// Send an event from an ISyncable Object.
        /// </summary>
        /// <param name="syncable">This is used to access parameters of the sender.</param>
        /// <param name="eventCode"></param>
        /// <param name="name">The name of the ScriptableObject. It is used to Load the Resource at the remote.</param>
        /// <param name="keyID">This is used to identify instanced Objects.</param>
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
        /// Receiving a photon event.
        /// </summary>
        /// <param name="eventcode"></param>
        /// <param name="content"></param>
        /// <param name="senderid"></param>
        void OnEvent(byte eventcode, object content, int senderid)
        {
            switch (eventcode)
            {
                case EventStateChange:

                    var contentAr = content as object[];
                    RemoteEnterState(contentAr[0] as string, ParseID(contentAr));
                    break;

                case EventRaiseUEGameEvent:

                    contentAr = content as object[];
                    RemoteGameEvent(contentAr[0] as string, ParseID(contentAr));
                    break;

                case EventRaiseUEFloatEvent:

                    contentAr = content as object[];
                    RemoteParameterEvent<float, FloatEvent>(contentAr[0] as string, ParseID(contentAr),
                        (float) contentAr[2]);
                    break;


                case EventRaiseUEStringEvent:

                    contentAr = content as object[];
                    RemoteParameterEvent<string, StringEvent>(contentAr[0] as string, ParseID(contentAr),
                        (string) contentAr[2]);
                    break;

                case EventRaiseUEBoolEvent:

                    contentAr = content as object[];
                    RemoteParameterEvent<bool, BoolEvent>(contentAr[0] as string, ParseID(contentAr),
                        (bool) contentAr[2]);
                    break;

                case EventRaiseUEIntEvent:

                    contentAr = content as object[];
                    RemoteParameterEvent<int, IntEvent>(contentAr[0] as string, ParseID(contentAr), (int) contentAr[2]);
                    break;

                case EventRaiseUEVector2Event:

                    contentAr = content as object[];
                    RemoteParameterEvent<Vector3, Vector3Event>(contentAr[0] as string, ParseID(contentAr),
                        (Vector2) contentAr[2]);
                    break;

                case EventRaiseUEVector3Event:

                    contentAr = content as object[];
                    RemoteParameterEvent<Vector3, Vector3Event>(contentAr[0] as string, ParseID(contentAr),
                        (Vector3) contentAr[2]);
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
            var gameEvent = Load<GameEvent>(cache, eventName, out success);
            if (!success) return;

            gameEvent.MuteNetworkBroadcasting = true;
            if (key == -1) gameEvent.Raise();
            else gameEvent.Raise(gameEvent.GetByKeyId(key));
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
            var paramEvent = Load<TS>(cache, eventName, out success);
            if (!success) return;

            paramEvent.MuteNetworkBroadcasting = true;
            if (key == -1) paramEvent.Raise(value);
            else paramEvent.Raise(value, paramEvent.GetByKeyId(key));
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
            var state = Load<State>(cache, stateName, out success);
            if (!success) return;

            state.stateManager.MuteNetworkBroadcasting = true;
            if (key == -1) state.Enter();
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
        public static T Load<T>(IDictionary<string, Object> cache, string key, out bool success) where T : Object
        {
            T asset;

            if (cache.ContainsKey(key))
            {
//            Logging.Log("Photon Sync", "Loading asset from dictionary.");
                asset = (T) cache[key];
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