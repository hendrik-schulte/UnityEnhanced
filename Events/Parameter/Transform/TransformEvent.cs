using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(Transform)")]
    public class TransformEvent : ParameterEvent<Transform, TransformEvent>
    {
        [SerializeField]
        private TransformUnityEvent OnTriggered = new TransformUnityEvent();

        protected override UnityEvent<Transform> OnEventTriggered => OnTriggered;
    }
}