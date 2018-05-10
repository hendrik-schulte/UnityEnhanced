// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UE_Photon
using UE.PUNNetworking;

#endif

namespace UE.Events
{
    /// <summary>
    /// An event that has sends parameter T with it. Needs to be inherited by a concrete ParameterEvent TS.
    /// Example: class BoolEvent : ParameterEvent<bool, BoolEvent>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TS"></typeparam>
    public abstract class ParameterEvent<T, TS> : InstanciableSO<TS>
        where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
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

        [SerializeField] private LogToFile logging = new LogToFile();

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
            Raise(value, null);
        }

        /// <summary>
        /// Fires an instanced event (or the main event if key == null) by the given instance key.
        /// </summary>
        public void Raise(T value, Object key = null)
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

        public void AddListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (!instance.eventListeners.Contains(listener))
                instance.eventListeners.Add(listener);
        }

        public void AddListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        public void RemoveListener(ParameterEventListener<T, TS> listener, Object key = null)
        {
            var instance = Instance(key);

            if (instance.eventListeners.Contains(listener))
                instance.eventListeners.Remove(listener);
        }

        public void RemoveListener(UnityAction<T> listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }

#if UNITY_EDITOR
    public class ParameterEventEditor<T, TS> : InstanciableSOEditor where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
        protected override string[] ExcludeProperties()
        {
            //Making sure the pun sync option is not displayed when the parameter is not syncable.
            var paramEvent = target as ParameterEvent<T, TS>;

            return paramEvent.IsNetworkingType ? base.ExcludeProperties() : new string[] {"PhotonSync"};
        }
#endif
    }
#endif
}