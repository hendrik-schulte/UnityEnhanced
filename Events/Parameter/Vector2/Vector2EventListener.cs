using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Vector2 Event Listener", 1)]
    public class Vector2EventListener : ParameterEventListener<Vector2, Vector2Event>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private Vector2Event Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private Vector2UnityEvent OnTriggered;

        protected override ParameterEvent<Vector2, Vector2Event> GenericEvent => Event;
        protected override UnityEvent<Vector2> GenericResponse => OnTriggered;
    }
}