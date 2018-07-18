using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [AddComponentMenu("Unity Enhanced/Events/Quaternion Event Listener", 1)]
    public class QuaternionEventListener : ParameterEventListener<Quaternion, QuaternionEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private QuaternionEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private QuaternionUnityEvent OnTriggered;

        protected override ParameterEvent<Quaternion, QuaternionEvent> GenericEvent => Event;
        protected override UnityEvent<Quaternion> GenericResponse => OnTriggered;
    }
}