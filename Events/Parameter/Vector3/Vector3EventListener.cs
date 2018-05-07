using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Vector3 Event Listener", 1)]
    public class Vector3EventListener : ParameterEventListener<Vector3, Vector3Event>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private Vector3Event Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private Vector3UnityEvent OnTriggered;

        protected override ParameterEvent<Vector3, Vector3Event> GenericEvent => Event;
        protected override UnityEvent<Vector3> GenericResponse => OnTriggered;
    }
}