using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(GameObject)")]
    public class GameObjectEvent : ParameterEvent<GameObject, GameObjectEvent>
    {
        [SerializeField]
        private GameObjectUnityEvent OnTriggered = new GameObjectUnityEvent();

        protected override UnityEvent<GameObject> OnEventTriggered => OnTriggered;
    }
}