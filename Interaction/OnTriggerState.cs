using UE.Instancing;
using UE.StateMachine;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTriggerState")]
    [RequireComponent(typeof(Collider))]
    public class OnTriggerState : OnTrigger
    {
        [SerializeField] private State state;

        protected override void Triggered(Component other)
        {
            base.Triggered(other);
            
            state.Enter(Key);
        }

        public override IInstanciable Target => !state ? null : state.stateManager;
    }
}