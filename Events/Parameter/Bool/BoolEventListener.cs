using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class BoolEventListener : ParameterEventListener<bool>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private BoolEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private BoolUnityEvent OnTriggered;

        protected override ParameterEvent<bool> GenericEvent => Event;
        protected override UnityEvent<bool> GenericResponse => OnTriggered;
    }
}