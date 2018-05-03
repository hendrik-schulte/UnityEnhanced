using UE.Events;
using UE.Instancing;
using UnityEngine;

namespace UE.Interaction
{
    public class SendTriggerEnterRandomFloat : OnTrigger
    {
        [SerializeField] private FloatEvent Event;

        [SerializeField] private float range = 2f;

        protected override void Triggered()
        {
            base.Triggered();
            
            Event.Raise(Random.value * range, key);
        }

        public override IInstanciable GetTarget()
        {
            return Event;
        }
    }
}