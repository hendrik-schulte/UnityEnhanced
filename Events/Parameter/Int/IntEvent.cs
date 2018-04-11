using UE.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(int)")]
    public class IntEvent : ParameterEvent<int, IntEvent>
    {
        [SerializeField]
        private IntUnityEvent OnTriggered = new IntUnityEvent();

        protected override UnityEvent<int> OnEventTriggered => OnTriggered;

        public void Raise(InputField inputField)
        {
            Raise(inputField.text.StringToInt());
        }
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}