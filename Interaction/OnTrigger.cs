using System.Linq;
using UE.Common;
using UE.Instancing;
using UE.StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTrigger")]
    [RequireComponent(typeof(Collider))]
    public class OnTrigger : InstanceObserver
    {
        [SerializeField] private TriggerStateEvent Mode;

        private enum TriggerStateEvent
        {
            OnTriggerEnter,
            OnTriggerExit,
            OnCollisionEnter,
            OnCollisionExit,
            OnCollisionEnter2D,
            OnCollisionExit2D,
        }

        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        [Header("Restrictions")]
        [Tooltip("The event only triggers when one of the following states is active (for all state instances).")]
        [SerializeField]
        private State[] onlyActiveIn;

        [SerializeField]
        [Tooltip("When this is checked, the states above are inverted. Thus the event " +
                 "only triggers when any state instance is not in the given states.")]
        private bool invertState;

#pragma warning disable 0108
        [Tooltip("The event only triggers when this is the other collider or null.")] [SerializeField]
        private Collider collider;
#pragma warning restore 0108
        
        [Tooltip("The event only triggers when the colliding GameObject has the given name.")] [SerializeField]
        private string colliderNameContains;

        [Tooltip("Disable this trigger for given seconds after it was triggered.")] [SerializeField] [Range(0, 5)]
        private float cooldown;

        private bool coolingDown;

        [Header("Response")] public UnityEvent OnTriggerEnterEvent;

        protected virtual void OnEnable()
        {
            coolingDown = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Mode == TriggerStateEvent.OnTriggerEnter) Trigger(other);
        }

        private void OnTriggerExit(Collider other)
        {
            if (Mode == TriggerStateEvent.OnTriggerExit) Trigger(other);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (Mode == TriggerStateEvent.OnCollisionEnter) Trigger(other.collider);            
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (Mode == TriggerStateEvent.OnCollisionExit) Trigger(other.collider);            
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Mode == TriggerStateEvent.OnCollisionEnter2D) Trigger(other.collider);            
        }
        
        private void OnCollisionExit2D(Collision2D other)
        {
            if (Mode == TriggerStateEvent.OnCollisionExit2D) Trigger(other.collider);            
        }

        private void Trigger(Component other)
        {
            Logging.Log(this, "Entered by " + other.gameObject.name, Logging.Level.Verbose, loggingLevel);

            if (!enabled || !gameObject.activeInHierarchy || coolingDown) return;

            if (!MeetsStateRestriction()) return;

            if (collider != null && collider != other) return;

            if (colliderNameContains.Any() && !other.gameObject.name.Contains(colliderNameContains)) return;

            Logging.Log(this, "Triggered by " + other.gameObject.name, Logging.Level.Info, loggingLevel);

            Triggered(other);
        }

        /// <summary>
        /// Returns true if the state restrictions are fulfilled.
        /// </summary>
        /// <returns></returns>
        private bool MeetsStateRestriction()
        {
            if (!onlyActiveIn.Any()) return true;

            var sm = onlyActiveIn[0].stateManager;
            
            if (invertState) return sm.AnyInstanceNotInEitherState(onlyActiveIn);
            else return sm.AllInstancesInEitherState(onlyActiveIn);
        }

        /// <summary>
        /// This function is called whenever the a collision meets the given requirements.
        /// It is meant to be overridden by subclasses for custom behaviour.
        /// </summary>
        protected virtual void Triggered(Component other)
        {
            coolingDown = true;
            Invoke(nameof(OnEnable), cooldown);
            
            OnTriggerEnterEvent.Invoke();
        }

        public override IInstanciable Target => null;

    }
}