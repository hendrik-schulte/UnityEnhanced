using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(Vector2)")]
    public class Vector2Event : ParameterEvent<Vector2, Vector2Event>
    {
        [SerializeField]
        private Vector2UnityEvent OnTriggered = new Vector2UnityEvent();

        protected override UnityEvent<Vector2> OnEventTriggered => OnTriggered;
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}