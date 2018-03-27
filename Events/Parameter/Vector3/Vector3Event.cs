using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(Vector3)")]
    public class Vector3Event : ParameterEvent<Vector3>
    {
        [SerializeField]
        private Vector3UnityEvent OnTriggered;

        protected override UnityEvent<Vector3> OnEventTriggered => OnTriggered;
    }
}