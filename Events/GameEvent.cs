// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UE_Photon
using UE.PUNNetworking;

#endif

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event()")]
    public class GameEvent : InstanciableSO<GameEvent>
    {
#if UE_Photon
        [SerializeField]
        private PhotonSync PhotonSync;

        protected override PhotonSync PhotonSyncSettings => PhotonSync;
        
        /// <summary>
        /// When this is true, events are not broadcasted. Used to avoid echoing effects.
        /// </summary>Broadcasting
        public bool MuteNetworkBroadcasting
        {
            get { return PhotonSync.MuteNetworkBroadcasting; }
            set { PhotonSync.MuteNetworkBroadcasting = value; }
        }
#endif
        
        [SerializeField] private LogToFile logging = new LogToFile();

        [SerializeField] private bool logToConsole;

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        [SerializeField] private UnityEvent OnEventTriggered = new UnityEvent();

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
            Raise(null);
        }

        /// <summary>
        /// Fires an instanced event (or the main event if key == null) by the given instance key.
        /// </summary>
        /// <param name="key"></param>
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
            
            FileLogger.Write(logging, name + " (" + instance.KeyID + ") was raised!");

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

        public void AddListener(GameEventListener listener, Object key = null)
        {
            if (!Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Add(listener);
        }

        public void AddListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        public void RemoveListener(GameEventListener listener, Object key = null)
        {
            if (Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Remove(listener);
        }

        public void RemoveListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameEvent), true)]
    [CanEditMultipleObjects]
    public class GameEventEditor : InstanciableSOEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameEvent = target as GameEvent;

            GUI.enabled = Application.isPlaying;

            if (gameEvent.Instanced)
            {
                if (GUILayout.Button("Raise for all Instances"))
                    gameEvent.RaiseAllInstances();
            }
            else
            {
                if (GUILayout.Button("Raise"))
                    gameEvent.Raise();
            }
        }
    }
#endif
}