using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(Object)")]
    public class ObjectEvent : ParameterEvent<Object, ObjectEvent>
    {
        [SerializeField]
        private ObjectUnityEvent OnTriggered = new ObjectUnityEvent();

        protected override UnityEvent<Object> OnEventTriggered => OnTriggered;
    }
}