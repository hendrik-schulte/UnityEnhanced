// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public abstract class ParameterEventListener<T> : MonoBehaviour
    {
        protected abstract ParameterEvent<T> GenericEvent { get; }
        protected abstract UnityEvent<T> GenericResponse { get; }

        [Tooltip("When this is checked, the listener will still work when the game object is disabled.")]
        public bool persistent;

        private void OnEnable()
        {
            if (!persistent) GenericEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (!persistent) GenericEvent.UnregisterListener(this);
        }

        private void Awake()
        {
            if (persistent) GenericEvent.RegisterListener(this);
        }

        private void OnDestroy()
        {
            if (persistent) GenericEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T value)
        {
            GenericResponse.Invoke(value);
        }
    }
}