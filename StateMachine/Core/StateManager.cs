using System;
using System.Runtime.InteropServices;
using Common;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{   
    [CreateAssetMenu(menuName = "State Machine/State Manager")]
    public class StateManager : ScriptableObject
    {

        [SerializeField] private bool debugLog;

        [Tooltip("The initial state of this system when the application is started")]
        public State InitialState;

        public StateChangeEvent OnStateEnter = new StateChangeEvent();
        public StateChangeEvent OnStateLeave = new StateChangeEvent();
        
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        
        [NonSerialized]
        private State _state;

        /// <summary>
        /// The current state of this system.
        /// </summary>
        public State State
        {
            set
            {
                if (_state == value) return;

                if (value == null)
                {
                    Debug.LogError("You are trying to set the state to null which is not supported!");                    
                    return;
                }
                
                if (value.stateManager != this)
                {
                    Debug.LogError("The state " + value.name + " you want to enter is not controlled by this state manager!");
                    return;
                }
                    
                if (debugLog) Logging.Log(this, "Change State to: " + value);
                OnStateLeave.Invoke(_state);
                _state = value;

                OnStateEnter.Invoke(value);
            }
            get { return _state; }
        }
        
        /// <summary>
        /// This is called when a StateListener starts.
        /// </summary>
        internal void Start()
        {
            if (State) return;

            if(InitialState) Logging.Log(this, "Initializing with " + InitialState.name, debugLog);
            else Logging.Log(this, "Initializing with null", debugLog);
            
            State = InitialState;
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