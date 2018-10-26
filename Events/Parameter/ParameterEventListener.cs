using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    /// <summary>
    /// Gerenric base class for listeners of events that pass a parameter.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    /// <typeparam name="TS">Type of the base class.</typeparam>
    public abstract class ParameterEventListener<T, TS> : InstanceObserver where TS : ParameterEvent<T, TS>
    {
        protected abstract ParameterEvent<T, TS> GenericEvent { get; }
        protected abstract UnityEvent<T> GenericResponse { get; }

        [Tooltip("When this is checked, the listener will still work when the game object is disabled.")]
        public bool persistent;

        protected virtual void OnEnable()
        {
            if (!persistent) GenericEvent.AddListener(this, Key);
        }

        protected virtual void OnDisable()
        {
            if (!persistent) GenericEvent.RemoveListener(this, Key);
        }

        protected virtual void Awake()
        {
            if (persistent) GenericEvent.AddListener(this, Key);
        }

        protected virtual void OnDestroy()
        {
            if (persistent) GenericEvent.RemoveListener(this, Key);
        }

        /// <summary>
        /// This is called when the listened event gets raised.
        /// </summary>
        public virtual void OnEventRaised(T value)
        {
            GenericResponse.Invoke(value);
        }
        
//        /// <summary>
//        /// Replaces the current event by the given one.
//        /// </summary>
//        /// <param name="paramEvent"></param>
//        public virtual void SetEvent(TS paramEvent)
//        {
//            if (Application.isPlaying)
//            {
//                if(GenericEvent != null) GenericEvent.RemoveListener(this);
//                paramEvent.AddListener(this);                
//            }
//            
//            GenericEvent = paramEvent;
//        }

        public override IInstanciable Target =>  GenericEvent;
    }
}