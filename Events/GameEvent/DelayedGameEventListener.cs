using UnityEngine;

namespace UE.Events
{
    /// <summary>
    /// This is an addition to the <see cref="GameEventListener"/> that allows
    /// you to introduce a delay to your event response. During
    /// the cooldown phase the response is not triggered.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Events/Delayed Game Event Listener", 1)]
    public class DelayedGameEventListener : GameEventListener
    {
        [SerializeField, Range(0, 10)] private float Delay;

        public override void OnEventRaised()
        {
            Invoke(nameof(RaiseEvent), Delay);   
        }

        private void RaiseEvent()
        {
            base.OnEventRaised();            
        }
    }
}