using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Object Event Listener", 1)]
    public class ObjectEventListener : ParameterEventListener<Object, ObjectEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private ObjectEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private ObjectUnityEvent OnTriggered;

        protected override ParameterEvent<Object, ObjectEvent> GenericEvent => Event;
        protected override UnityEvent<Object> GenericResponse => OnTriggered;
    }
}