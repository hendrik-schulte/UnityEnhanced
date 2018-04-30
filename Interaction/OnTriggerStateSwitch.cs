using UE.Common;
using UE.Instancing;
using UE.StateMachine;
using UnityEngine;

namespace UE.Interaction
{
    [RequireComponent(typeof(Collider))]
    public class OnTriggerStateSwitch : OnTrigger
    {
        [SerializeField] private State stateA;
        [SerializeField] private State stateB;

        protected override void Triggered()
        {
            if(stateB.IsActive(key)) 
                stateA.Enter(key);
            else if(stateA.IsActive(key)) 
                stateB.Enter(key);
            else Logging.Log(this, "This component assumes that it is used for a " +
                                   "two-state system with one state always enabled.", 
                Logging.Level.Warning, loggingLevel);
        }

        public override IInstanciable GetTarget()
        {
            return !stateA ? null : stateA.stateManager;
        }
    }
}