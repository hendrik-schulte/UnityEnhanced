using UE.Instancing;
using UE.StateMachine;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnOrientationState")]
    public class OnOrientationState : OnOrientation
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