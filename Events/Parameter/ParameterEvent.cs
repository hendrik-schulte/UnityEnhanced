// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
#if UE_Photon
using UE.PUNNetworking;

#endif

namespace UE.Events
{
    /// <inheritdoc />
    /// <summary>
    /// An event that has sends parameter T with it. Needs to be inherited by a
    /// concrete <see cref="ParameterEvent{T,TS}"/>.
    /// Example implementation: <see cref="BoolEvent"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TS"></typeparam>
    public abstract class ParameterEvent<T, TS> : InstanciableSO<TS>
        where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
        /// <summary>
        /// Settings for photon sync.
        /// </summary>
        [SerializeField] private PhotonSync PhotonSync;

        public override PhotonSync PhotonSyncSettings => PhotonSync;

        /// <summary>
        /// When this is true, events are not broadcasted. Used to avoid echoing effects.
        /// </summary>Broadcasting
        public bool MuteNetworkBroadcasting
        {
            get { return PhotonSync.MuteNetworkBroadcasting; }
            set { PhotonSync.MuteNetworkBroadcasting = value; }
        }

        /// <summary>
        /// This is true if the parameter type is serializable for photon.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNetworkingType => false;
#endif

        /// <summary>
        /// Settings for file logging.
        /// </summary>
        [SerializeField] private LogToFile logging = new LogToFile();

        [Tooltip("Should this event be logged to the console (not in Build)?")]
        [SerializeField] private bool logToConsole;

        [Multiline] public string DeveloperDescription = "";

        protected abstract UnityEvent<T> OnEventTriggered { get; }

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<ParameterEventListener<T, TS>> eventListeners =
            new List<ParameterEventListener<T, TS>>();

        /// <summary>
        /// Raises the event. Does only work for non-instanced events.
        /// </summary>
        /// <param name="value"></param>
        public void Raise(T value)
        {
            // Splitting functions to be able to call this from serialized functions calls.
            // ReSharper disable once IntroduceOptionalParameters.Global
            Raise(value, null);
        }

        /// <summary>
        /// Fires an instanced event (or the main event if key == null) by the given instance key.
        /// </summary>
        public void Raise(T value, Object key)
        {
            if (logToConsole) Debug.Log("Parameter Event '" + name + "' was raised with parameter: '" + value + "'!");

            RaiseInstance(Instance(key), value);
        }

        /// <summary>
        /// Fires the given event instance.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        private void RaiseInstance(ParameterEvent<T, TS> instance, T value)
        {
            for (var i = instance.eventListeners.Count - 1; i >= 0; i--)
                instance.eventListeners[i].OnEventRaised(value);
            instance.OnEventTriggered.Invoke(value);

            if(Instanced) FileLogger.Write(logging, 
                name + " was raised with parameter " + value + ".", 
                instance.KeyID.ToString());
            else FileLogger.Write(logging, name + " was raised with parameter " + value + ".");
            
#if UE_Photon
            PhotonSyncManager.SendEventParam(PhotonSync, name, instance.KeyID, value);
#endif
        }

        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances(T value)
        {
            Logging.Log(this, name + " was raised for all instances with value: " + value, logToConsole);

            RaiseInstance(this, value);

            if (!Instanced) return;

            foreach (var instance in GetInstances())
            {
                RaiseInstance(instance, value);
            }
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (!instance.eventListeners.Contains(listener))
                instance.eventListeners.Add(listener);
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (instance.eventListeners.Contains(listener))
                instance.eventListeners.Remove(listener);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }
}