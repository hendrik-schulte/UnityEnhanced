using UE.Instancing;
using UE.StateMachine;
using UnityEngine;

namespace UE.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class OnTriggerState : OnTrigger
    {
        [SerializeField] private State state;

        protected override void Triggered()
        {
            state.Enter(key);
        }

        public override IInstanciable GetTarget()
        {
            return !state ? null : state.stateManager;
        }
    }
}