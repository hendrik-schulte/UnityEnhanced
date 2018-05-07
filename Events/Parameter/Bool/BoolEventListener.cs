using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Bool Event Listener", 1)]
    public class BoolEventListener : ParameterEventListener<bool, BoolEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private BoolEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private BoolUnityEvent OnTriggered;

        protected override ParameterEvent<bool, BoolEvent> GenericEvent => Event;
        protected override UnityEvent<bool> GenericResponse => OnTriggered;
    }
}