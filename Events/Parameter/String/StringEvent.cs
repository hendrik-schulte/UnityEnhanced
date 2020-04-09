using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(string)")]
    public class StringEvent : ParameterEvent<string, StringEvent>
    {
        [SerializeField]
        private StringUnityEvent OnTriggered = new StringUnityEvent();

        protected override UnityEvent<string> OnEventTriggered => OnTriggered;
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}