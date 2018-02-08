using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    /// <summary>
    /// This component activates this game object when one of the given states
    /// is activated and disables it as soon as the state is left.
    /// </summary>
    public class ActiveInState : MonoBehaviour
    {
        [SerializeField] private List<State> activeStates;

        [Tooltip("When this is set the first state in the activeStates list will be activated at start.")]
        [SerializeField]
        private bool enabledOnStart;

        public UnityEvent OnActivated;
        public UnityEvent OnDeactivated;

        void Start()
        {
            if (!activeStates.Any()) return;

            var stateManager = activeStates[0].stateManager;

            stateManager.OnStateEnter.AddListener(OnStateEnter);

            if (enabledOnStart) activeStates[0].stateManager.State = activeStates[0];
            else gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the single player window as soon as the state is set.
        /// </summary>
        /// <param name="state"></param>
        private void OnStateEnter(State state)
        {
            var previouslyActive = gameObject.activeSelf;
            var active = activeStates.Any(s => s == state);
            
            gameObject.SetActive(active);
            
            
            if(previouslyActive && !active) OnDeactivated.Invoke();
            if(!previouslyActive && active) OnActivated.Invoke();
        }
    }
}