using System;
using UE.Instancing;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.StateMachine
{
    /// <summary>
    /// This class wraps states to enable instancing without taking care of the instance keys.
    /// The keys are defined in the inspector in case of an instanced state machine.
    /// </summary>
    [Serializable]
    public class InstancedState : InstanceReference
    {
        public State state;

        public override IInstanciable Target => state?.stateManager;

        /// <summary>
        /// Enters the initial state of this state machine if no other state has been set.
        /// </summary>
        public void Init()
        {
            state.stateManager.Init(Key);
        }
        
        /// <summary>
        /// Enters this state.
        /// </summary>
        public void Enter()
        {
            state.Enter(Key);
        }
        
        /// <summary>
        /// Enters this state for all instances of this state machine.
        /// </summary>
        public void EnterAllInstances()
        {
            state.stateManager.SetStateAllInstances(state);
        }

        /// <summary>
        /// Returns true if this state is currently activated.
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return state.IsActive(Key);
        }
    }
}