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
            if (stateB.IsActive(key))
            {
                base.Triggered(other);
                stateA.Enter(key);
            }
            else if (stateA.IsActive(key))
            {
                base.Triggered(other);
                stateB.Enter(key);
            }
            else
            {
                Logging.Log(this, "This component assumes that it is used for a " +
                                  "two-state system with one state always enabled.",
                    Logging.Level.Warning, loggingLevel);
            }
        }

        public override IInstanciable GetTarget()
        {
            return !stateA ? null : stateA.stateManager;
        }
    }
}