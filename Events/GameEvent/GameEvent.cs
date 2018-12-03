// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Common;
using UE.Instancing;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;
using UnityEngine.Events;
#if UE_Photon
using UE.PUNNetworking;

#endif

namespace UE.Events
{
    /// <inheritdoc />
    /// <summary>
    /// A global event that can be observed by a <see cref="T:UE.Events.GameEventListener"/>.
    /// It is created as an asset file and can be raised from anywhere in you project.
    /// </summary>
    [CreateAssetMenu(menuName = "Events/Event()", order = -1)]
    public class GameEvent : InstanciableSO<GameEvent>
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
#endif

        /// <summary>
        /// Settings for file logging.
        /// </summary>
        [SerializeField] private LogToFile logging = new LogToFile();

        [Tooltip("Should this event be logged to the console (not in Build)?")]
        [SerializeField] private bool logToConsole;

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        [SerializeField] private UnityEvent OnEventTriggered = new UnityEvent();
        
        /// <summary>
        /// Override this to disable the UnityEvents in the inspector.
        /// </summary>
        public virtual bool DrawUnityEventInspector => true;

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>i
        private readonly List<GameEventListener> eventListeners =
            new List<GameEventListener>();

        /// <summary>
        /// Raises the event. Does only work for non-instanced events.
        /// </summary>
        public void Raise()
        {
            // Splitting functions to be able to call this from serialized functions calls.
            // ReSharper disable once IntroduceOptionalParameters.Global
            Raise(null);
        }

        /// <summary>
        /// Fires an instanced event (or the main event if key == null) by the given instance key.
        /// </summary>
        /// <param name="key">Key for instanced events.</param>
        public void Raise(Object key)
        {
            Logging.Log(this, name + " was raised!", logToConsole);

            RaiseInstance(Instance(key));
        }

        /// <summary>
        /// Fires the given event instance.
        /// </summary>
        /// <param name="instance"></param>
        private void RaiseInstance(GameEvent instance)
        {
            for (var i = instance.eventListeners.Count - 1; i >= 0; i--)
                instance.eventListeners[i].OnEventRaised();
            instance.OnEventTriggered.Invoke();

            if (Instanced)
                FileLogger.Write(logging,
                    name + " was raised.",
                    instance.KeyID.ToString());
            else FileLogger.Write(logging, name + " was raised.");

#if UE_Photon
            PhotonSyncManager.SendEvent(PhotonSync, PhotonSyncManager.EventRaiseUEGameEvent, name, instance.KeyID);
#endif
        }

        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances()
        {
            Logging.Log(this, name + " was raised for all instances!", logToConsole);

            RaiseInstance(this);

            if (!Instanced) return;

            foreach (var instance in GetInstances())
            {
                RaiseInstance(instance);
            }
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(GameEventListener listener, Object key = null)
        {
            var listeners = Instance(key).eventListeners;
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Register a listener to this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void AddListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(GameEventListener listener, Object key = null)
        {
            var listeners = Instance(key).eventListeners;
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        /// <summary>
        /// Remove a listener from this event.
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="key">Key for instanced events</param>
        public void RemoveListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Raise the event when clicking it.
        /// </summary>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var instance = EditorUtility.InstanceIDToObject(instanceID) as GameEvent;

            if (instance == null) return false;

            if (Application.isPlaying)
            {
                EditorGUIUtility.PingObject(instance);
                instance.RaiseAllInstances();
            }
            
            return false;                
        }
#endif
    }
}