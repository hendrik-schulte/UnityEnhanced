using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class Vector3EventListener : ParameterEventListener<Vector3>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private Vector3Event Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private Vector3UnityEvent OnTriggered;

        protected override ParameterEvent<Vector3> GenericEvent => Event;
        protected override UnityEvent<Vector3> GenericResponse => OnTriggered;
    }
}