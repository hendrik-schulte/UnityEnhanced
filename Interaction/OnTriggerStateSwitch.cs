using UE.Common;
using UE.Instancing;
using UE.StateMachine;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTriggerStateSwitch")]
    [RequireComponent(typeof(Collider))]
    public class OnTriggerStateSwitch : OnTrigger
    {
        [SerializeField] private State stateA;
        [SerializeField] private State stateB;

        protected override void Triggered(Component other)
        {
            if (stateB.IsActive(Key))
            {
                base.Triggered(other);
                stateA.Enter(Key);
            }
            else if (stateA.IsActive(Key))
            {
                base.Triggered(other);
                stateB.Enter(Key);
            }
            else
            {
                Logging.Log(this, "This component assumes that it is used for a " +
                                  "two-state system with one state always enabled.",
                    Logging.Level.Warning, loggingLevel);
            }
        }

        public override IInstanciable Target => !stateA ? null : stateA.stateManager;
    }
}