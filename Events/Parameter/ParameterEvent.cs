// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public abstract class ParameterEvent<T, TS> : InstanciableSO<TS> where TS : ParameterEvent<T, TS>
    {
        [SerializeField] private bool debugLog;

        [Multiline] public string DeveloperDescription = "";

        protected abstract UnityEvent<T> OnEventTriggered { get; }

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<ParameterEventListener<T, TS>> eventListeners =
            new List<ParameterEventListener<T, TS>>();

        public void Raise(T value)
        {
            Raise(value, null);
        }

        public void Raise(T value, Object key = null)
        {
            if (debugLog) Debug.Log("Parameter Event '" + name + "' was raised with parameter: '" + value + "'!");

            var instance = Instance(key);

            for (var i = instance.eventListeners.Count - 1; i >= 0; i--)
                instance.eventListeners[i].OnEventRaised(value);
            instance.OnEventTriggered.Invoke(value);
        }

        public void RegisterListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (!instance.eventListeners.Contains(listener))
                instance.eventListeners.Add(listener);
        }

        public void RegisterListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        public void UnregisterListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (instance.eventListeners.Contains(listener))
                instance.eventListeners.Remove(listener);
        }

        public void UnregisterListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }
}