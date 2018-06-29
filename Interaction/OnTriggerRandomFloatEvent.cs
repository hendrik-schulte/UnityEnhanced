using UE.Events;
using UE.Instancing;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTriggerRandomFloatEvent")]
    public class SendTriggerEnterRandomFloat : OnTrigger
    {
        [SerializeField] private FloatEvent Event;

        [SerializeField] private float range = 2f;

        protected override void Triggered(Component other)
        {
            base.Triggered(other);
            
            Event.Raise(Random.value * range, Key);
        }

        public override IInstanciable Target => Event;
    }
}