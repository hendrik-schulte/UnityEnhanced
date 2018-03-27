using UnityEngine;

namespace UE.StateMachine
{
    [CreateAssetMenu(menuName = "State Machine/State")]
    public class State : ScriptableObject
    {
        [Tooltip("This is the manager in wich this state is managed.")]
        public StateManager stateManager;

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        /// <summary>
        /// This state is activated.
        /// </summary>
        public void Enter()
        {
            stateManager.State = this;
        }

        /// <summary>
        /// Returns true if this state is currently activated.
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            if (!stateManager) return false;

            return stateManager.State == this;
        }

        /// <summary>
        /// This returns true, when the state is the initial state of this system.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialState()
        {
            if (!stateManager) return false;

            return stateManager.InitialState == this;
        }
    }
}