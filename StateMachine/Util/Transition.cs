using System.Collections;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.StateMachine
{
    /// <summary>
    /// A Transition is used to automatically move to annother state when a condition is met.
    /// </summary>
    public abstract class Transition : InstanceObserver
    {
        [SerializeField] protected State transitState;
        [SerializeField] protected State followingState;

        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        [SerializeField] private bool EnterFollowingStateOnDisable;

        [SerializeField] private UnityEvent OnTransitionStart;
        [SerializeField] private UnityEvent OnTransitionComplete;


        protected virtual void Awake()
        {
#if UNITY_EDITOR
            if (transitState.stateManager != followingState.stateManager)
            {
                Logging.Log(this, "The states do not belong to the same state machine!",
                    Logging.Level.Error, loggingLevel);
                return;
            }

            Logging.Log(this, "Adding Listener", Logging.Level.Verbose, loggingLevel);
#endif

            transitState.stateManager.AddStateEnterListener(OnStateEnter, Key);
            
            transitState.stateManager.Init(Key);
        }

        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            Logging.Log(this, "Removing Listener", Logging.Level.Verbose, loggingLevel);
#endif

            transitState.stateManager.RemoveStateEnterListener(OnStateEnter, Key);
        }

        private void OnStateEnter(State state)
        {
            StopAllCoroutines();

            if (state != transitState) return;

            Logging.Log(this, "Starting transition ...", Logging.Level.Info, loggingLevel);

            StartTransition(true);
        }

        protected void StartTransition(bool triggerEvent)
        {
            if (!gameObject.activeInHierarchy || !enabled) return;

            if (triggerEvent) OnTransitionStart.Invoke();
            StopAllCoroutines();
            StartCoroutine(TransitionStart());
        }

        protected abstract IEnumerator TransitionStart();

        protected void TransitionComplete()
        {
            if (!transitState.IsActive(Key))
            {
#if UNITY_EDITOR
                Logging.Log(this,
                    "Transition completed but transit state is not active anymore!",
                    Logging.Level.Warning, loggingLevel);
#endif

                return;
            }

#if UNITY_EDITOR
            Logging.Log(this, "Transition completed!", Logging.Level.Info, loggingLevel);
#endif

            OnTransitionComplete.Invoke();
            followingState.Enter(Key);
        }

        protected virtual void OnDisable()
        {
            if (EnterFollowingStateOnDisable)
            {
                followingState.Enter(Key);
            }
        }

        public override IInstanciable Target => !transitState ? null : transitState.stateManager;
    }
}