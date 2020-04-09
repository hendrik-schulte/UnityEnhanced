using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(Quaternion)")]
    public class QuaternionEvent : ParameterEvent<Quaternion, QuaternionEvent>
    {
        [SerializeField]
        private QuaternionUnityEvent OnTriggered = new QuaternionUnityEvent();

        protected override UnityEvent<Quaternion> OnEventTriggered => OnTriggered;
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}