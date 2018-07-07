using System;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    /// <inheritdoc />
    [Serializable]
    public class InstancedGameEvent : InstanceReference
    {
        [SerializeField] private GameEvent gameEvent;

        public override IInstanciable Target => gameEvent;

        /// <summary>
        /// Raises the event.
        /// </summary>
        public void Raise()
        {
            gameEvent.Raise(Key);
        }
        
        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances()
        {
            gameEvent.RaiseAllInstances();
        }
        
        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(GameEventListener listener)
        {
            gameEvent.AddListener(listener, Key);
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(UnityAction listener)
        {
            gameEvent.AddListener(listener, Key);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(GameEventListener listener)
        {
            gameEvent.RemoveListener(listener, Key);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(UnityAction listener)
        {
            gameEvent.RemoveListener(listener, Key);
        }
    }
}