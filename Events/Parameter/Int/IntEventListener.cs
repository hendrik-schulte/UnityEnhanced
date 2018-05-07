using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Int Event Listener", 1)]
    public class IntEventListener : ParameterEventListener<int, IntEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private IntEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private IntUnityEvent OnTriggered;

        protected override ParameterEvent<int, IntEvent> GenericEvent => Event;
        protected override UnityEvent<int> GenericResponse => OnTriggered;
    }
}