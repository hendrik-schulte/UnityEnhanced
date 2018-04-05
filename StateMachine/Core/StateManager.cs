using System;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace UE.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/State Manager")]
    public class StateManager : InstanciableSO<StateManager>
    {
        [SerializeField] private bool debugLog;

        private StateChangeEvent OnStateEnter = new StateChangeEvent();
        private StateChangeEvent OnStateLeave = new StateChangeEvent();

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        [Tooltip("The initial state of this system when the application is started")]
        public State InitialState;

        [NonSerialized] private State _state;

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
        /// Returns true if this system has an initial state defined.
        /// </summary>
        /// <returns></returns>
        public bool HasInitialState()
        {
            return InitialState != null;
        }
    }
}