using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(Object)")]
    public class ObjectEvent : ParameterEvent<Object, ObjectEvent>
    {
        [SerializeField]
        private ObjectUnityEvent OnTriggered = new ObjectUnityEvent();

        protected override UnityEvent<Object> OnEventTriggered => OnTriggered;
    }
}