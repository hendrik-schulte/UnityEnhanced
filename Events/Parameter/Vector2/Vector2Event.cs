using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(Vector2)")]
    public class Vector2Event : ParameterEvent<Vector2>
    {
        [SerializeField]
        private Vector2UnityEvent OnTriggered;

        protected override UnityEvent<Vector2> OnEventTriggered => OnTriggered;
    }
}