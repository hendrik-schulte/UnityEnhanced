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

        public void Raise(InputField inputField)
        {
            Raise(inputField.text.StringToFloat());
        }
    }
}