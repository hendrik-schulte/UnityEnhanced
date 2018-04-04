// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public abstract class ParameterEventListener<T, TS> : InstanceObserver where TS : ParameterEvent<T, TS>
    {
        protected abstract ParameterEvent<T, TS> GenericEvent { get; }
        protected abstract UnityEvent<T> GenericResponse { get; }

        [Tooltip("When this is checked, the listener will still work when the game object is disabled.")]
        public bool persistent;

        private void OnEnable()
        {
            if (!persistent) GenericEvent.RegisterListener(this, key);
        }

        private void OnDisable()
        {
            if (!persistent) GenericEvent.UnregisterListener(this, key);
        }

        private void Awake()
        {
            if (persistent) GenericEvent.RegisterListener(this, key);
        }

        private void OnDestroy()
        {
            if (persistent) GenericEvent.UnregisterListener(this, key);
        }

        public void OnEventRaised(T value)
        {
            GenericResponse.Invoke(value);
        }

        public override IInstanciable GetTarget()
        {
            return GenericEvent;
        }
    }
}