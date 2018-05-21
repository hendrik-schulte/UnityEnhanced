using UnityEngine;

namespace UE.Events
{
    /// <summary>
    /// This is an addition to the <see cref="GameEventListener"/> that allows
    /// you to introduce a cooldown to your event response. During
    /// the cooldown phase the response is not triggered.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Events/Cooldown Game Event Listener", 1)]
    public class CooldownGameEventListener : GameEventListener
    {
        [SerializeField, Range(0, 10)] private float Cooldown;

        private bool coolingDown;

        protected override void OnEnable()
        {
            CooledDown();
            base.OnEnable();
        }

        public override void OnEventRaised()
        {
            if (coolingDown) return;
            
            coolingDown = true;
            Invoke(nameof(CooledDown), Cooldown);
            base.OnEventRaised();
        }

        private void CooledDown()
        {
            coolingDown = false;
        }
    }
}