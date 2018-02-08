using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu]
    public class State : ScriptableObject
    {
        [Tooltip("This is the manager in wich this state is managed.")]
        public StateManager stateManager;

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
    }
}