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
            state.Enter(Key);
        }

        public override IInstanciable Target =>  !state ? null : state.stateManager;
    }
}