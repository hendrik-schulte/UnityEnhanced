using System;
using UE.Instancing;
using UnityEngine.Events;

namespace UE.Events
{
    /// <inheritdoc />
    [Serializable]
    public abstract class InstancedParameterEvent<T, TS> : InstanceReference
        where TS : ParameterEvent<T, TS>
    {
        public TS paramEvent;

        public override IInstanciable Target => paramEvent;

        /// <summary>
        /// Raises the event.
        /// </summary>
        public void Raise(T value)
        {
            paramEvent.Raise(value, Key);
        }
        
        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances(T value)
        {
            paramEvent.RaiseAllInstances(value);
        }
        
        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(ParameterEventListener<T, TS> listener)
        {
            paramEvent.AddListener(listener, Key);
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(UnityAction<T> listener)
        {
            paramEvent.AddListener(listener, Key);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(ParameterEventListener<T, TS> listener)
        {
            paramEvent.RemoveListener(listener, Key);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(UnityAction<T> listener)
        {
            paramEvent.RemoveListener(listener, Key);
        }
    }
}