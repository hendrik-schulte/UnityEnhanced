using UE.Events;
using UE.Instancing;
using UE.Interaction;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Interaction
{
    public class SendTriggerEnterRandomFloat : OnTrigger
    {
        [SerializeField] private FloatEvent Event;

        [SerializeField] private float range = 2f;

        protected override void Triggered()
        {
            Event.Raise(Random.value * range, key);
        }

        public override IInstanciable GetTarget()
        {
            return Event;
        }
    }
}