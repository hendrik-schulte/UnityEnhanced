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
    /// <summary>
    /// The StateManager is the core of every state machine. It manages the current state
    /// of the system and offers listeners to state enter and leave events.
    /// </summary>
    [CreateAssetMenu(menuName = "State Machine/State Manager")]
    public class StateManager : InstanciableSO<StateManager>
    {
#if UE_Photon
        [SerializeField] private PhotonSync PhotonSync;

        public override PhotonSync PhotonSyncSettings => PhotonSync;

        /// <summary>
        /// When this is true, events are not broadcasted. Used to avoid echoing effects.
        /// </summary>Broadcasting
        public bool MuteNetworkBroadcasting
        {
            get { return PhotonSync.MuteNetworkBroadcasting; }
            set { PhotonSync.MuteNetworkBroadcasting = value; }
        }

        /// <summary>
        /// Returns true when Photon sync is enabled.
        /// </summary>
        public bool PUNSyncEnabled => PhotonSync.PUNSync;
#endif

        /// <summary>
        /// Settings for file logging.
        /// </summary>
        [SerializeField] 
        private LogToFile fileLogging = new LogToFile();

        /// <summary>
        /// Returns true if file logging is enabled.
        /// </summary>
        public bool FileLoggingEnabled => fileLogging.logToFile;

        /// <summary>
        /// This returns true when console logging is enabled.
        /// </summary>
        public bool ConsoleLoggingEnabled => logToConsole;

        /// <summary>
        /// Logging to console enabled.
        /// </summary>
        [SerializeField] 
        private bool logToConsole;

        /// <summary>
        /// This event is triggered when a state is entered.
        /// Provides the new state as parameter.
        /// </summary>
        private StateEvent OnStateEnter = new StateEvent();
        
        /// <summary>
        /// This event is triggered when a state is left.
        /// Provides the left and upcoming state as parameter.
        /// </summary>
        private StateStateEvent OnStateLeave = new StateStateEvent();

#if UNITY_EDITOR
#pragma warning disable 0414  
        [SerializeField, Multiline] private string DeveloperDescription = "";
#pragma warning restore 0414        
#endif

        [Tooltip("The initial state of this system when the application is started.")]
        public State InitialState;

        private void Awake()
        {
            //When copying a state manager, check for initial state not 
            //to reference a state of the original state machine.
            if (InitialState != null)
            {
                if(InitialState.stateManager == this) return;

                InitialState = null;
            }
        }

        /// <summary>
        /// The current state of this state machine.
        /// </summary>
        [NonSerialized] private State _state;

        /// <summary>
        /// Enters the given state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void SetState(State state, Object key = null)
        {
            SetStateInstance(Instance(key), state, key);
        }

        /// <summary>
        /// Enters the given state in the given StateManager instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="state"></param>
        /// <param name="key">Key for instanced StateMachine.</param>
        private void SetStateInstance(StateManager instance, State state, Object key = null)
        {
            if (instance._state == state) return; //The state to enter is already active.

            if (state == null)
            {
                Logging.Error(this, "You are trying to set the state to null which is not supported!");
                return;
            }

            if (state.stateManager != this)
            {
                Logging.Error(this, "The state " + state.name + " you want to enter " +
                                    "is not controlled by this state manager '"+ name +"'!");
                return;
            }
            
#if UNITY_EDITOR
            Logging.Log(this, "Change State to: " + state, logToConsole);
#endif
      
            instance.OnStateLeave.Invoke(instance._state, state);
            instance._state = state;
            instance.OnStateEnter.Invoke(state);

            if(Instanced) FileLogger.Write(fileLogging, 
                state.name + " was entered.", 
                instance.KeyID.ToString());
            else FileLogger.Write(fileLogging, state.name + " was entered.");

#if UE_Photon
            PhotonSyncManager.SendEvent(PhotonSync, PhotonSyncManager.EventStateChange, state.name, instance.KeyID);
#endif
        }

        /// <summary>
        /// Sets the given state for all instances.
        /// </summary>
        /// <param name="state"></param>
        public void SetStateAllInstances(State state)
        {
            SetStateInstance(this, state);

            if (!Instanced) return;

            foreach (var instance in GetInstances())
            {
                SetStateInstance(instance, state);
            }
        }

        /// <summary>
        /// Returns the current state of this state machine.
        /// </summary>
        /// <param name="key">Key for instanced StateMachine.</param>
        /// <returns></returns>
        public State GetState(Object key = null)
        {
            return Instance(key)._state;
        }

        /// <summary>
        /// Register a listener to the OnStateEntered event. Provides the entered state as parameter.
        /// </summary>
        /// <param name="action">The Action to perform.</param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void AddStateEnterListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.AddListener(action);
        }

        /// <summary>
        /// Remove a listener from the OnStateEntered event.
        /// </summary>
        /// <param name="action">The Action to remove.</param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void RemoveStateEnterListener(UnityAction<State> action, Object key = null)
        {
            Instance(key).OnStateEnter.RemoveListener(action);
        }

        /// <summary>
        /// Register a listener to the OnStateLeave event. Provides the left and the upcoming state as parameter.
        /// </summary>
        /// <param name="action">The Action to perform.</param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void AddStateLeaveListener(UnityAction<State, State> action, Object key = null)
        {
            Instance(key).OnStateLeave.AddListener(action);
        }
        
        /// <summary>
        /// Remove a listener from the OnStateLeave event.
        /// </summary>
        /// <param name="action">The Action to remove.</param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void RemoveStateLeaveListener(UnityAction<State, State> action, Object key = null)
        {
            Instance(key).OnStateLeave.RemoveListener(action);
        }

        /// <summary>
        /// This is called when a StateListener starts. Used for initialization of this state machine.
        /// </summary>
        /// <param name="key">Key for instanced StateMachine.</param>
        internal void Init(Object key = null)
        {
            if (GetState(key)) return;

            if (InitialState) Logging.Log(this, "Initializing with " + InitialState.name, logToConsole);
            else Logging.Log(this, "Initializing with null", logToConsole);

            SetState(InitialState, key);
        }

        /// <summary>
        /// Returns true if all state machine instances are currently in either of the given states.
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
        /// Returns true if there if any state machine instance is in a state not given.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public bool AnyInstanceNotInEitherState(params State[] states)
        {
            if (!states.Any()) return true;

            if (states.Any(state => state.stateManager != this))
            {
                Logging.Warning(this, "The states checked do not belong to this state machine.");
                return false;
            }

            if (!Instanced)
                return !states.Contains(_state);

            foreach (var instance in GetInstances())
            {
                if (!states.Contains(instance._state)) return true;
            }

            return false;
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
        /// <param name="key">Key for instanced StateMachine.</param>
        public void DrawWorldSpaceGizmo(Vector3 position, Object key = null)
        {
            DrawWorldSpaceGizmo(position, null, key);
        }

        /// <summary>
        /// This draws a state gizmo at the given world space position.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="color">Color of the gizmo.</param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void DrawWorldSpaceGizmo(Vector3 position, Color? color = null, Object key = null)
        {
            var state = GetState(key);

            if (!Application.isPlaying)
            {
                if (HasInitialState())
                    InitialState.DrawWorldSpaceGizmo(position, color);
                else
                    Gizmo.DrawWorldSpaceString("Starting with: Null", position, color);

                return;
            }

            if (!state) Gizmo.DrawWorldSpaceString("Current: Null", position, color);
            else state.DrawWorldSpaceGizmo(position, color);
        }
    }
}