using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UnityEngine;
using UnityEngine.Events;

namespace UE.StateMachine
{
    /// <summary>
    /// This component sends events when the given states are activated. Can be inherited for special applications.
    /// </summary>
    public class StateListener : MonoBehaviour
    {
        [SerializeField] 
        protected bool debugLog;
        [SerializeField] 
        private List<State> activeStates;

        [SerializeField]
        protected UnityEvent OnActivated;
        [SerializeField]
        protected UnityEvent OnDeactivated;

        /// <summary>
        /// Returns true if this is currently activated.
        /// </summary>
        public bool Active { get; private set; }

        void Start()
        {
            if (!activeStates.Any())
            {
                enabled = false;
                Logging.Warning(this, "'" + gameObject.name + "': There are no active states defined!");
                return;
            }

            var stateManager = activeStates[0].stateManager;

            stateManager.Start();
            
            stateManager.OnStateEnter.AddListener(OnStateEnter);

            if (IsActiveState(stateManager.State))
            {
                if (debugLog) Logging.Log(this, "'" + gameObject.name + "' Activated");
                Active = true;
                Activated();
                OnActivated.Invoke();
            }
            else
            {
                if (debugLog) Logging.Log(this, "'" + gameObject.name + "' Deactivated");
                OnDeactivated.Invoke();
                Deactivated(true);
            }
        }

        /// <summary>
        /// Returns true if the given state is one of the active states.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool IsActiveState(State state)
        {
            return activeStates.Any(s => s == state);
        }

        /// <summary>
        /// Activates the single player window as soon as the state is set.
        /// </summary>
        /// <param name="state"></param>
        private void OnStateEnter(State state)
        {
            var previouslyActive = Active;
            Active = IsActiveState(state);

            if (!previouslyActive && Active)
            {
                if (debugLog) Logging.Log(this, "'" + gameObject.name + "' Activated");
                Activated();
                OnActivated.Invoke();
            }
            if (previouslyActive && !Active)
            {
                if (debugLog) Logging.Log(this, "'" + gameObject.name + "' Deactivated");
                OnDeactivated.Invoke();
                Deactivated();
            }
        }
        
        /// <summary>
        /// This is called at start and when one of the active states is entered and the previous state was not one
        /// of the active states. This can be overridden by sub classes to induce custom behaviour when the current
        /// state is activated.
        /// </summary>
        protected virtual void Activated()
        {   
        }
        
        /// <summary>
        /// This is called at start and when one of the active states is left and the next state is not one
        /// of the active states. This can be overridden by sub classes to induce custom behaviour when the current
        /// state is deactivated.
        /// </summary>
        /// <param name="atStart">This is true for immediate disabling at start.</param>
        protected virtual void Deactivated(bool atStart = false)
        {
        }
        
        private void OnDestroy()
        {
            if (!activeStates.Any()) return;

            activeStates[0].stateManager.OnStateEnter.RemoveListener(OnStateEnter);
        }
    }
}