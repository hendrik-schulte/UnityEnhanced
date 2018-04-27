﻿// ----------------------------------------------------------------------------
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
#if UE_Photon
        , ISyncable
#endif
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
#endif
        
        [SerializeField, HideInInspector]
        [Tooltip("Enables automatic logging of this event to a file.")]
        private bool LogToFile;
        [SerializeField, HideInInspector]
        [Tooltip("Name of the log file.")]
        private string FileName = "main.log";
        
        [SerializeField] private bool debugLog;

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
            Logging.Log(this, name + " was raised!", debugLog);

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
            
            if(LogToFile) FileLogger.Write(FileName, name + " (" + instance.KeyID + ") was raised!");

#if UE_Photon
            PhotonSync.SendEvent(this, PhotonSync.EventRaiseUEGameEvent, name, instance.KeyID);
#endif
        }

        /// <summary>
        /// This raises the event for all instances.
        /// </summary>
        public void RaiseAllInstances()
        {
            Logging.Log(this, name + " was raised for all instances!", debugLog);

            RaiseInstance(this);

            if (!Instanced) return;

            foreach (var instance in GetInstances())
            {
                RaiseInstance(instance);
            }
        }

        public void RegisterListener(GameEventListener listener, Object key = null)
        {
            if (!Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Add(listener);
        }

        public void RegisterListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        public void UnregisterListener(GameEventListener listener, Object key = null)
        {
            if (Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Remove(listener);
        }

        public void UnregisterListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameEvent), true)]
    [CanEditMultipleObjects]
    public class GameEventEditor : InstanciableSOEditor
    {
#if UE_Photon
        private SerializedProperty PUNSync;
        private SerializedProperty CachingOptions;
#endif
        private SerializedProperty LogToFile;
        private SerializedProperty FileName;
        

        protected override void OnEnable()
        {
            base.OnEnable();

#if UE_Photon
            PUNSync = serializedObject.FindProperty("PUNSync");
            CachingOptions = serializedObject.FindProperty("cachingOptions");
#endif
            LogToFile = serializedObject.FindProperty("LogToFile");
            FileName = serializedObject.FindProperty("FileName");
        }

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

        protected override void OnInspectorGUITop()
        {
#if UE_Photon
            serializedObject.Update();
            PhotonEditorUtility.PhotonControl(PUNSync, CachingOptions);
            serializedObject.ApplyModifiedProperties();
#endif
            serializedObject.Update();
            FileLogger.LoggerControl(LogToFile, FileName);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}