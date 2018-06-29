using UE.Events;
using UE.Instancing;
using UnityEngine;

namespace UE.Interaction
{
    [AddComponentMenu("Unity Enhanced/Interaction/OnTriggerEvent")]
    [RequireComponent(typeof(Collider))]
    public class OnTriggerEvent : OnTrigger
    {
        [SerializeField] private GameEvent gameEvent;

        protected override void Triggered(Component other)
        {
            base.Triggered(other);
            
            gameEvent.Raise(Key);
        }

        public override IInstanciable Target => gameEvent;
    }
}