using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.StateMachine
{
    /// <summary>
    /// This component sends events when the given states are activated. Can be inherited for special applications.
    /// </summary>
    public class StateListener : InstanceObserver
    {
#if UNITY_EDITOR
        [SerializeField] protected bool debug;
#endif
        [SerializeField] private List<State> activeStates = new List<State>() {null};

        [SerializeField] protected UnityEvent OnActivated;
        [SerializeField] protected UnityEvent OnDeactivated;

        /// <summary>
        /// Returns true if this is currently activated.
        /// </summary>
        public bool Active { get; private set; }

        protected virtual void Start()
        {
            //check if there are any states defined
            if (!HasStates())
            {
                enabled = false;
                Logging.Warning(this, "'" + gameObject.name + "': There are no active states defined!");
                return;
            }

            var stateManager = activeStates[0].stateManager;

#if UNITY_EDITOR
            //check if all states are in the same state system
            foreach (var state in activeStates)
            {
                if (state.stateManager == stateManager) continue;

                enabled = false;
                Logging.Warning(this,
                    "'" + transform.GetTransformHierachy() + "': Not all states belong to the same state machine!");
                return;
            }

            //check if instance key is assigned
            if (stateManager.Instanced && key == null)
            {
                Logging.Warning(this, "'" + transform.GetTransformHierachy() + "': The instance key is not defined!");
            }
#endif

            stateManager.Init(key);

            stateManager.AddStateEnterListener(OnStateEnter, key);

            if (IsActiveState(stateManager.GetState(key)))
            {
#if UNITY_EDITOR
                Logging.Log(this, "'" + transform.GetTransformHierachy() + "' Activated", debug);
#endif
                Active = true;
                Activated();
                OnActivated.Invoke();
            }
            else
            {
#if UNITY_EDITOR
                Logging.Log(this, "'" + transform.GetTransformHierachy() + "' Deactivated", debug);
#endif
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
#if UNITY_EDITOR
                Logging.Log(this, "'" + transform.GetTransformHierachy() + "' Activated", debug);
#endif
                Activated();
                OnActivated.Invoke();
            }

            if (previouslyActive && !Active)
            {
#if UNITY_EDITOR
                Logging.Log(this, "'" + transform.GetTransformHierachy() + "' Deactivated", debug);
#endif
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

        protected virtual void OnDestroy()
        {
            if (!HasStates()) return;

#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Removing Listener", debug);
#endif

            activeStates[0].stateManager.RemoveStateEnterListener(OnStateEnter, key);
        }

        public override IInstanciable GetTarget()
        {
            if (!activeStates.Any()) return null;

            if (activeStates[0] == null) return null;

            return activeStates[0].stateManager;
        }

        /// <summary>
        /// Returns true if there are states defined and the first state is not null.
        /// </summary>
        /// <returns></returns>
        protected bool HasStates()
        {
            return activeStates.Any() && activeStates[0] != null;
        }
#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (!debug || !HasStates() || !Application.isPlaying) return;

            activeStates[0].stateManager?.DrawWorldSpaceGizmo(transform.position, key);
        }
#endif
    }
}