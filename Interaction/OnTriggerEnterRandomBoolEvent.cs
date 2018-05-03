using UE.Events;
using UE.Instancing;
using UnityEngine;

namespace UE.Interaction
{
    public class SendTriggerEnterRandomBool : OnTrigger
    {
        [SerializeField] private BoolEvent Event;

        protected override void Triggered()
        {
            base.Triggered();
            
            Event.Raise(Random.value > 0.5f,  key);
        }

        public override IInstanciable GetTarget()
        {
            return Event;
        }
    }
}