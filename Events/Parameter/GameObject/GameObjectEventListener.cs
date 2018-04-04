using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class GameObjectEventListener : ParameterEventListener<GameObject, GameObjectEvent>
    {
        [SerializeField]
        [Tooltip("Event to register with.")] 
        private GameObjectEvent Event;

        [SerializeField]
        [Tooltip("Response to invoke when Event is raised.")] 
        private GameObjectUnityEvent OnTriggered;

        protected override ParameterEvent<GameObject, GameObjectEvent> GenericEvent => Event;
        protected override UnityEvent<GameObject> GenericResponse => OnTriggered;
    }
}