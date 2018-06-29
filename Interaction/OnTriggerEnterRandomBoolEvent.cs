using UE.Events;
using UE.Instancing;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTriggerRandoBoolEvent")]
    public class SendTriggerEnterRandomBool : OnTrigger
    {
        [SerializeField] private BoolEvent Event;

        protected override void Triggered(Component other)
        {
            base.Triggered(other);
            
            Event.Raise(Random.value > 0.5f,  Key);
        }

        public override IInstanciable Target => Event;
    }
}