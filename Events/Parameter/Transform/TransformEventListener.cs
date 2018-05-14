using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Transform Event Listener", 1)]
    public class TransformEventListener : ParameterEventListener<Transform, TransformEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private TransformEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private TransformUnityEvent OnTriggered;

        protected override ParameterEvent<Transform, TransformEvent> GenericEvent => Event;
        protected override UnityEvent<Transform> GenericResponse => OnTriggered;
    }
}