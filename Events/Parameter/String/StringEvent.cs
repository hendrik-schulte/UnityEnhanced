using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Event(string)")]
    public class StringEvent : ParameterEvent<string>
    {
        [SerializeField]
        private StringUnityEvent OnTriggered;

        protected override UnityEvent<string> OnEventTriggered => OnTriggered;
    }
}