using UE.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(float)")]
    public class FloatEvent : ParameterEvent<float, FloatEvent>
    {
        [SerializeField]
        private FloatUnityEvent OnTriggered = new FloatUnityEvent();

        protected override UnityEvent<float> OnEventTriggered => OnTriggered;

        /// <summary>
        /// Raises this event with the input fields text parsed to float as parameter.
        /// </summary>
        /// <param name="inputField"></param>
        public void Raise(InputField inputField)
        {
            Raise(inputField.text.StringToFloat());
        }
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}