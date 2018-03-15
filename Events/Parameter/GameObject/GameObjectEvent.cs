using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Event(GameObject)")]
    public class GameObjectEvent : ParameterEvent<GameObject>
    {
        [SerializeField]
        private GameObjectUnityEvent OnTriggered;

        protected override UnityEvent<GameObject> OnEventTriggered => OnTriggered;
    }
}