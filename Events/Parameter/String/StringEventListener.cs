using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class StringEventListener : ParameterEventListener<string, StringEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private StringEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private StringUnityEvent OnTriggered;

        protected override ParameterEvent<string, StringEvent> GenericEvent => Event;
        protected override UnityEvent<string> GenericResponse => OnTriggered;
    }
}