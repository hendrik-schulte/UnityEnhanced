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

        protected override void Triggered()
        {
            base.Triggered();
            
            state.Enter(key);
        }

        public override IInstanciable GetTarget()
        {
            return !state ? null : state.stateManager;
        }
    }
}