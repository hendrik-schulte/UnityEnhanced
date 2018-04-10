using System;
using System.Linq;
using UE.Common;
using UE.Instancing;
using UE.UI;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UE_Photon
using UE.PUNNetworking;
#endif

namespace UE.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/State Manager")]
    public class StateManager : InstanciableSO<StateManager>
    {
#if UE_Photon
        [SerializeField, HideInInspector] 
        [Tooltip("Enables automatic sync of this state machine in a Photon network.\n" +
                 "You need to have a PhotonSync Component in your Scene and the " +
                 "state assets needs to be located at the root of a resources folder" +
                 "with a unique name.")]
        private bool PUNSync;
        
        [SerializeField, HideInInspector] private EventCaching CachingOptions;
        
        /// <summary>
        /// When this is true, events are not broadcasted. Used to avoid echoing effects.
        /// </summary>
        [NonSerialized]
        public bool MuteNetworkBroadcasting;
#endif
        
        [SerializeField] private bool debugLog;

        private StateChangeEvent OnStateEnter = new StateChangeEvent();
        private StateChangeEvent OnStateLeave = new StateChangeEvent();

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        [Tooltip("The initial state of this system when the application is started.")]
        public State InitialState;

        [NonSerialized] private State _state;

        /// <summary>
        /// Enters the given state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void SetState(State state, Object key = null)
        {
            var instance = Instance(key);


            if (instance._state == state) return;

            if (state == null)
            {
                Logging.Error(this, "You are trying to set the state to null which is not supported!");
                return;
            }

            if (state.stateManager != this)
            {
                Logging.Error(this, "The state " + state.name + " you want to enter " +
                                    "is not controlled by this state manager!");
                return;
            }

            if (debugLog) Logging.Log(this, "Change State to: " + state);

            instance.OnStateLeave.Invoke(_state);
            instance._state = state;

            instance.OnStateEnter.Invoke(state);
            
#if UE_Photon
            if (PUNSync && PhotonNetwork.inRoom && !MuteNetworkBroadcasting )
            {
                var raiseEventOptions = new RaiseEventOptions()
                {
                    CachingOption = CachingOptions,
                    Receivers = ReceiverGroup.Others
                };

                PhotonNetwork.RaiseEvent(PhotonSync.EventStateChange, name, true, raiseEventOptions);
            }
#endif
        }

        public State GetState(Object key = null)
        {
            return Instance(key)._state;
        }

        public void AddStateEnterListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.AddListener(action);
        }

        public void RemoveStateEnterListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.RemoveListener(action);
        }

        public void AddStateLeaveListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.AddListener(action);
        }

        public void RemoveStateLeaveListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.RemoveListener(action);
        }

        /// <summary>
        /// This is called when a StateListener starts.
        /// </summary>
        internal void Init(Object key = null)
        {
            if (GetState(key)) return;

            if (InitialState) Logging.Log(this, "Initializing with " + InitialState.name, debugLog);
            else Logging.Log(this, "Initializing with null", debugLog);

            SetState(InitialState, key);
        }

        /// <summary>
        /// Returns true if all state machine instances are currently in this state.
        /// </summary>
        /// <returns></returns>
        public bool AllInstancesInEitherState(params State[] states)
        {
            if (!states.Any()) return true;

            if (states.Any(state => state.stateManager != this))
            {
                Logging.Warning(this, "The states checked do not belong to this state machine.");
                return false;
            }

            if (!Instanced)
                return states.Any(state => state.IsActive());

            return GetInstances().All(v => states.Contains(v.GetState()));
        }

        /// <summary>
        /// Returns true if this system has an initial state defined.
        /// </summary>
        /// <returns></returns>
        public bool HasInitialState()
        {
            return InitialState != null;
        }

        /// <summary>
        /// This draws a state gizmo at the given world space position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="key"></param>
        public void DrawWorldSpaceGizmo(Vector3 position, Object key = null)
        {
            var state = GetState(key);

            if (!Application.isPlaying)
            {
                if (HasInitialState())
                    InitialState.DrawWorldSpaceGizmo(position);
                else
                    Gizmo.DrawWorldSpaceString("Starting with: Null", position);
                return;
            }

            if (!state) Gizmo.DrawWorldSpaceString("Current: Null", position);
            else state.DrawWorldSpaceGizmo(position);
        }
    }
}