using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class FloatEventListener : ParameterEventListener<float>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private FloatEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private FloatUnityEvent OnTriggered;

        protected override ParameterEvent<float> GenericEvent => Event;
        protected override UnityEvent<float> GenericResponse => OnTriggered;
    }
}