using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public class GameObjectEventListener : ParameterEventListener<GameObject>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private GameObjectEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private GameObjectUnityEvent OnTriggered;

        protected override ParameterEvent<GameObject> GenericEvent => Event;
        protected override UnityEvent<GameObject> GenericResponse => OnTriggered;
    }
}