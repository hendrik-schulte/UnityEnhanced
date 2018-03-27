using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class StringEventListener : ParameterEventListener<string>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private StringEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private StringUnityEvent OnTriggered;

        protected override ParameterEvent<string> GenericEvent => Event;
        protected override UnityEvent<string> GenericResponse => OnTriggered;
    }
}