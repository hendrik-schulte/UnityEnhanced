using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(bool)")]
    public class BoolEvent : ParameterEvent<bool, BoolEvent>
    {
        [SerializeField]
        private BoolUnityEvent OnTriggered = new BoolUnityEvent();

        protected override UnityEvent<bool> OnEventTriggered => OnTriggered;
    }
}