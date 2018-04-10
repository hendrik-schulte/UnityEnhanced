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
        public const byte EventRaiseUEEvent = 102;


        void OnEnable()
        {
            PhotonNetwork.OnEventCall += OnEvent;

//        foreach (var stateMachine in syncStateMachines)
//        {
//            if (stateMachine.Instanced)
//            {
//                stateMachine.OnInstancesChanged.AddListener(OnStateMachineChanged);
//                OnStateMachineChanged(stateMachine);
//            }
//            else
//                stateMachine.AddStateEnterListener(OnStateEnter);
//        }
        }

        void OnDisable()
        {
            PhotonNetwork.OnEventCall -= OnEvent;

//        foreach (var stateMachine in syncStateMachines)
//        {
//            if (stateMachine.Instanced)
//                stateMachine.OnInstancesChanged.RemoveListener(OnStateMachineChanged);
//            else
//                stateMachine.RemoveStateEnterListener(OnStateEnter);
//        }
        }


        void OnEvent(byte eventcode, object content, int senderid)
        {
            switch (eventcode)
            {
                case EventStateChange:

                    RemoteEnterState(content as string);
                    break;

                case EventRaiseUEEvent:

                    RemoteEvent(content as string);
                    break;
            }
        }

//    private void OnGameEventInstanceChanged(GameEvent gameEvent)
//    {
//        foreach (var instance in gameEvent.GetInstances())
//        {
////            instance.RemoveStateEnterListener(OnInstancedStateEnter);
////            instance.AddStateEnterListener(OnInstancedStateEnter);
//        }
//    }

        private void RemoteEvent(string eventName)
        {
            Logging.Log(this, eventName + " GameEvent received!", debugLog);

            bool success;
            var gameEvent = Load(eventCache, eventName, out success);

            if (!success) return;

            gameEvent.Raise();
        }


//    private void OnGameEventRaised(GameEvent gameEvent)
//    {
//        if(gameEvent == disabledEvent) return;
//        
//        var raiseEventOptions = new RaiseEventOptions()
//        {
//            CachingOption = EventCaching.DoNotCache,
//            Receivers = ReceiverGroup.Others
//        };
//
//        PhotonNetwork.RaiseEvent(EventStateChange, gameEvent.name, true, raiseEventOptions);
//    }

        #region State Machine

//    private void OnStateEnter(State state)
//    {
//        var raiseEventOptions = new RaiseEventOptions()
//        {
//            CachingOption = EventCaching.DoNotCache,
//            Receivers = ReceiverGroup.Others
//        };
//
//        PhotonNetwork.RaiseEvent(EventStateChange, state.name, true, raiseEventOptions);
//    }

//    private void OnInstancedStateEnter(State state)
//    {
//    }

        private void RemoteEnterState(string stateName)
        {
            Logging.Log(this, "State Enter Event received!", debugLog);

            bool success;
            var state = Load(stateCache, stateName, out success);

            if (!success) return;

            state.stateManager.MuteNetworkBroadcasting = true;
            state.Enter();
            state.stateManager.MuteNetworkBroadcasting = false;
        }

//    private void OnStateMachineChanged(StateManager stateManager)
//    {
//        foreach (var instance in stateManager.GetInstances())
//        {
//            instance.RemoveStateEnterListener(OnInstancedStateEnter);
//            instance.AddStateEnterListener(OnInstancedStateEnter);
//        }
//    }

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
//            Logging.Log(this, "Loading asset from dictionary.", debugLog);
                asset = cache[key];
            }
            else
            {
//            Logging.Log(this, "Can't find the asset in the cache. Calling Resources.Load ...", debugLog);
                asset = Resources.Load<T>(key);
                cache.Add(key, asset);
            }

            if (!asset)
            {
//            Logging.Error(this, "The desired asset does not exist.");
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