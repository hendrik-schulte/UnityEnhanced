using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event(bool)")]
    public class BoolEvent : ParameterEvent<bool, BoolEvent>
    {
        [SerializeField]
        private BoolUnityEvent OnTriggered = new BoolUnityEvent();

        protected override UnityEvent<bool> OnEventTriggered => OnTriggered;

        /// <summary>
        /// Raises this event with true as parameter.
        /// </summary>
        public void RaiseTrue()
        {
            Raise(true);
        }
        
        /// <summary>
        /// Raises this event with with false as parameter.
        /// </summary>
        public void RaiseFalse()
        {
            Raise(false);
        }
        
        /// <summary>
        /// Raises this event with true as parameter.
        /// </summary>
        /// <param name="key">Instancing Key</param>
        public void RaiseTrue(Object key = null)
        {
            Raise(true, key);
        }
        
        /// <summary>
        /// Raises this event with false as parameter
        /// </summary>
        /// <param name="key">Instancing Key</param>
        public void RaiseFalse(Object key = null)
        {
            Raise(false, key);
        }
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}