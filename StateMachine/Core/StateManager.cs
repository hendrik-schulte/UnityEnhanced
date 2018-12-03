using System;
using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

#if UE_Photon
using UE.PUNNetworking;

#endif

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// The StateManager is the core of every state machine. It manages which <see cref="State"/> is
    /// currently active in this system. Can be observed by <see cref="StateListener"/>s.
    /// </summary>
    [CreateAssetMenu(menuName = "State Machine/State Manager", order = -1)]
    public class StateManager : InstanciableSO<StateManager>
    {
#if UE_Photon
/// <summary>
/// Settings for photon sync.
/// </summary>
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
        [SerializeField] private LogToFile fileLogging = new LogToFile();

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
        [SerializeField] internal bool logToConsole;

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

        /// <summary>
        /// This state will be entered as soon as the <see cref="Init"/> function is called for the first time.
        /// </summary>
        [Tooltip("The initial state of this system when the application is started.")]
        public State InitialState;

        private void Awake()
        {
            //Take care of the asset not being unloaded within scene changes.
            hideFlags = HideFlags.DontUnloadUnusedAsset;

#if UNITY_EDITOR
            //When copying a state manager, check for initial state not 
            //to reference a state of the original state machine.
            if (InitialState == null || InitialState.stateManager == this) return;

            InitialState = null;
#endif
        }

        /// <summary>
        /// The current state of this state machine.
        /// </summary>
        [NonSerialized] private State _state;


        /// <summary>
        /// The history of the previously entered states. Used to go back to the previous state.
        /// </summary>
        [NonSerialized] private List<State> history;

        private const int MAX_HISTORY_DEPTH = 10;

        /// <summary>
        /// Enters the given state.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void SetState(State state, Object key = null)
        {
            SetStateInstance(Instance(key), state);
        }

        /// <summary>
        /// Enters the given state.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(InstancedState state)
        {
            SetStateInstance(Instance(state.Key), state.state);
        }

        /// <summary>
        /// Enters the given state in the given StateManager instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="state"></param>
        /// <param name="key">Key for instanced StateMachine.</param>
        private void SetStateInstance(StateManager instance, State state)
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
                                    "is not controlled by this state manager '" + name + "'!");
                return;
            }

#if UNITY_EDITOR
            Logging.Log(this, "Change State to: " + state, logToConsole);
#endif

            instance.OnStateLeave.Invoke(instance._state, state);
            instance._state = state;
            instance.OnStateEnter.Invoke(state);

            instance.WriteHistory();

            if (Instanced)
                FileLogger.Write(fileLogging,
                    state.name + " was entered.",
                    instance.KeyID.ToString());
            else FileLogger.Write(fileLogging, state.name + " was entered.");

#if UE_Photon
            PhotonSyncManager.SendEvent(PhotonSync, PhotonSyncManager.EventStateChange, state.name, instance.KeyID);
#endif
        }

        /// <summary>
        /// Writes the current state to the state history.
        /// </summary>
        private void WriteHistory()
        {
            if (history == null)
            {
                history = new List<State>{_state};
                return;
            }

            if (history.Last() == _state) return;

            history.Add(_state);

            while (history.Count > MAX_HISTORY_DEPTH) history.RemoveAt(0);
        }

#if UE_Photon
        /// <summary>
        /// Sends a state change message to all clients. This is called
        /// when a new client joins so it has all states synchronized.
        /// </summary>
        public void PropagateStatePhoton()
        {
            if (Instanced)
                foreach (var instance in GetInstances())
                    instance.PropagateStatePhotonInstance();

            else if (_state != null)
                PhotonSyncManager.SendEvent(PhotonSync, PhotonSyncManager.EventStateChange, _state.name, KeyID);
        }
        
        /// <summary>
        /// Propagates the current state of this instance towards all other clients.
        /// </summary>
        private void PropagateStatePhotonInstance()
        {
            if (_state != null)
                PhotonSyncManager.SendEvent(original.PhotonSync, PhotonSyncManager.EventStateChange, _state.name, KeyID);
        }
#endif

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
        /// This enters the initial state at startuo. If you rely on an initial state this must be called by any
        /// MonoBehaviour at least once on Start. StateListeners do this automatically. If you don't use StateListeners
        /// you need to call this method manually. It is required because ScriptableObject do not receive application
        /// start events.
        /// </summary>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void Init(Object key = null)
        {
            if (GetState(key)) return;

#if UNITY_EDITOR
            if (InitialState) Logging.Log(this, "Initializing with " + InitialState.name, logToConsole);
            else Logging.Log(this, "Initializing with null", logToConsole);
#endif

#if UE_Photon
//Register main instance to sync manager to automatically propagate state changes to new players.
            if (PhotonSync.PUNSync)
                if (Instanced && original)
                    PhotonSyncManager.RegisterStateManager(original);
                else
                    PhotonSyncManager.RegisterStateManager(this);
#endif

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
        /// Returns true if there is a state to go back to.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool HasPreviousState(Object key = null)
        {
            var instance = Instance(key);

            return instance.history != null && instance.history.Count >= 2;
        }

        /// <summary>
        /// Returns the previous state or null if there is none.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public State GetPreviousState(Object key = null)
        {
            if (!HasPreviousState(key)) return null;

            var instance = Instance(key);
            return instance.history[instance.history.Count - 2];
        }

        /// <summary>
        /// Go back to the previously entered state.
        /// </summary>
        public void Back()
        {
            Back(null);
        }

        /// <summary>
        /// Go back to the previously entered state.
        /// </summary>
        /// <param name="key">Key for instanced StateMachine.</param>
        public void Back(Object key)
        {
            var instance = Instance(key);

            if (!HasPreviousState(key))
            {
                Logging.Warning(this, "Back was clicked but there is no state left in the history to go back to.");
                return;
            }

            instance.history.RemoveAt(instance.history.Count - 1);

            instance.SetState(instance.history.Last());
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
#if UNITY_EDITOR
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
#endif
        }
    }
}