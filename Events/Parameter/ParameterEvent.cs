// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public abstract class ParameterEvent<T> : ScriptableObject
    {        
        [SerializeField] 
        private bool debugLog;

        [Multiline]
        public string DeveloperDescription = "";

        protected abstract UnityEvent<T> OnEventTriggered
        {
            get;
        }

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<ParameterEventListener<T>> eventListeners = 
            new List<ParameterEventListener<T>>();

        public void Raise(T value)
        {
            if(debugLog) Debug.Log("Parameter Event '" + name + "' was raised with parameter: '" + value + "'!");
            
            for(var i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised(value);
            OnEventTriggered.Invoke(value);
        }

        public void RegisterListener(ParameterEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void RegisterListener(UnityAction<T> listener)
        {
            OnEventTriggered.AddListener(listener);
        }

        public void UnregisterListener(ParameterEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
        
        public void UnregisterListener(UnityAction<T> listener)
        {
            OnEventTriggered.RemoveListener(listener);   
        }
    }
}