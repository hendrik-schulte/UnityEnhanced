using System;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    [Serializable]
    public class StateChangeEvent : UnityEvent<State>
    {
    }
    
    [CreateAssetMenu]
    public class StateManager : ScriptableObject
    {

        [SerializeField] private bool debugLog;

        public StateChangeEvent OnStateEnter = new StateChangeEvent();
        public StateChangeEvent OnStateLeave = new StateChangeEvent();
        
        private State _state;

        public State State
        {
            set
            {
                if (_state == value) return;
                
                
                if (value != null && value.stateManager != this)
                {
                    Debug.LogWarning("The state " + value.name + " you want to enter is not controlled by this state manager!");
                    return;
                }
                    
                if (debugLog) Debug.Log("Change State to: " + value);
                OnStateLeave.Invoke(_state);
                _state = value;

                OnStateEnter.Invoke(value);
            }
            get { return _state; }
        }
    }
}