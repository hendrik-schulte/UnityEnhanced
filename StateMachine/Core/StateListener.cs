using System.Collections.Generic;
using System.Linq;
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.StateMachine
{
    /// <summary>
    /// This component sends events when the given states are activated. Can be inherited for special applications.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/State Listener", 0)]
    public class StateListener : InstanceObserver
    {
#if UNITY_EDITOR
        [SerializeField] protected bool debug;
#endif
        [SerializeField] [Reorderable] protected List<State> activeStates = new List<State>();

        [SerializeField] protected UnityEvent OnActivated;
        [SerializeField] protected UnityEvent OnDeactivated;

        /// <summary>
        /// Override this to disable the UnityEvents in the inspector.
        /// </summary>
        public virtual bool DrawUnityEventInspector => true;

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
                Logging.Warning(this, "There are no active states defined!");
                return;
            }

            var stateManager = activeStates[0].stateManager;

#if UNITY_EDITOR

            //check if all states are in the same state system
            if (!StatesShareStateManager(activeStates))
            {
                enabled = false;
                Logging.Warning(this,
                    "'" + transform.GetTransformHierachy() + "': Not all states belong to the same state machine!");
                return;
            }

            //check if instance key is assigned
            if (stateManager.Instanced && Key == null)
            {
                Logging.Warning(this, "'" + transform.GetTransformHierachy() + "': The instance key is not defined!");
            }
#endif

            stateManager.Init(Key);

            stateManager.AddStateEnterListener(OnStateEnter, Key);
            stateManager.AddStateLeaveListener(OnStateLeft, Key);

            if (IsActiveState(stateManager.GetState(Key)))
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
        }

        /// <summary>
        /// This is called when a state is left.
        /// </summary>
        /// <param name="leftState"></param>
        /// <param name="upcomingState"></param>
        private void OnStateLeft(State leftState, State upcomingState)
        {
            if (IsActiveState(leftState) && !IsActiveState(upcomingState))
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

            //when leaving play mode, the key object may already be destroyed
            //so we avoid using the wrong key and return.
            if (activeStates[0].stateManager.Instanced && Key == null) return;

            activeStates[0].stateManager.RemoveStateEnterListener(OnStateEnter, Key);
            activeStates[0].stateManager.RemoveStateLeaveListener(OnStateLeft, Key);
        }

        public override IInstanciable Target
        {
            get
            {
                if (!activeStates.Any()) return null;

                return activeStates[0] == null ? null : activeStates[0].stateManager;
            }
        }

        /// <summary>
        /// Returns true if there are states defined and the first state is not null.
        /// </summary>
        /// <returns></returns>
        protected bool HasStates()
        {
            return activeStates.Any() && activeStates[0] != null;
        }

        /// <summary>
        /// Returns true if all given states share the same StateManager.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        internal static bool StatesShareStateManager(List<State> states)
        {
            if (!states.Any()) return true;

            foreach (var state in states)
            {
                if (state == null || state.stateManager == states[0].stateManager) continue;

                return false;
            }

            return true;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            if (!debug || !HasStates() || !Application.isPlaying) return;

            activeStates[0].stateManager?.DrawWorldSpaceGizmo(transform.position, Key);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StateListener), true)]
    [CanEditMultipleObjects]
    public class StateListenerEditor : InstanceObserverEditor
    {
        private SerializedProperty activeStates;

        protected override void InitInspector()
        {
            base.InitInspector();

            activeStates = serializedObject.FindProperty("activeStates");
        }

        protected override void DrawInspector()
        {
            var activeStatesList = activeStates.ToList<State>();

            if (!(activeStatesList.Any() && activeStatesList[0] != null))

                EditorGUILayout.HelpBox("You need to define at least one active state for this listener to work.",
                    MessageType.Error);

            else if (!StateListener.StatesShareStateManager(activeStatesList))

                EditorGUILayout.HelpBox("Your states do not share the same StateManager. This is not supported.",
                    MessageType.Error);


            base.DrawInspector();
        }

        protected override IEnumerable<string> ExcludeProperties()
        {
            if ((target as StateListener).DrawUnityEventInspector)
                return base.ExcludeProperties();
            else
                return new[] {"OnActivated", "OnDeactivated"};
        }
    }

#endif
}