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
#if UE_Photon
        , ISyncable
#endif
        where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
        [SerializeField, HideInInspector]
        [Tooltip("Enables automatic sync of this event in a Photon network. " +
                 "You need to have a PhotonSync Component in your Scene and the " +
                 "event asset needs to be located at the root of a Resources folder " +
                 "with a unique name.")]
        private bool PUNSync;

        [SerializeField, HideInInspector] [Tooltip("How should the event be cached by Photon?")]
        private EventCaching cachingOptions;

        //Implementing ISyncable
        public bool PUNSyncEnabled => PUNSync;
        public EventCaching CachingOptions => cachingOptions;
        public bool MuteNetworkBroadcasting { get; set; }
        
        /// <summary>
        /// This is true if the parameter type is serializable for photon.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsNetworkingType => false;
#endif

        [SerializeField] private bool debugLog;

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
            if (debugLog) Debug.Log("Parameter Event '" + name + "' was raised with parameter: '" + value + "'!");

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

#if UE_Photon
            PhotonSync.SendEventParam(this, name, instance.KeyID, value);
#endif
        }
        
        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances(T value)
        {
            Logging.Log(this, name + " was raised for all instances with value: " + value, debugLog);
            
            RaiseInstance(this, value);
            
            if(!Instanced) return;
            
            foreach (var instance in GetInstances())
            {
                RaiseInstance(instance, value);
            }
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
    
#if UNITY_EDITOR
    public class ParameterEventEditor<T, TS> : InstanciableSOEditor where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
        private SerializedProperty PUNSync;
        private SerializedProperty CachingOptions;
#endif

        protected override void OnEnable()
        {
            base.OnEnable();

#if UE_Photon
            PUNSync = serializedObject.FindProperty("PUNSync");
            CachingOptions = serializedObject.FindProperty("cachingOptions");
#endif
        }

        protected override void OnInspectorGUITop()
        {
#if UE_Photon
            var paramEvent = target as ParameterEvent<T, TS>;

            if (!paramEvent.IsNetworkingType) return;
            
            serializedObject.Update();
            ScriptableObjectUtility.PhotonControl(PUNSync, CachingOptions);
            serializedObject.ApplyModifiedProperties();
#endif
        }
    }
#endif
}