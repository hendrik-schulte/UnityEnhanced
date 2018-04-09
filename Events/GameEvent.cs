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

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event()")]
    public class GameEvent : InstanciableSO<GameEvent>
    {
        [SerializeField] private bool debugLog;

#if UE_Photon
        [SerializeField]
        private bool PhotonSync;
#endif
        
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
        /// Raises the event. Does not work with instancing.
        /// </summary>
        public void Raise()
        {
            Raise(null);
        }

        public void Raise(Object key)
        {
            if (debugLog) Debug.Log("Game Event '" + name + "' was raised!");
            
            for (var i = Instance(key).eventListeners.Count - 1; i >= 0; i--)
                Instance(key).eventListeners[i].OnEventRaised();
            Instance(key).OnEventTriggered.Invoke();
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
    public class EventEditor : InstanciableSOEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameEvent = target as GameEvent;
            
            GUI.enabled = Application.isPlaying && !gameEvent.Instanced;

            if (GUILayout.Button("Raise"))
                gameEvent.Raise();
        }
    }
#endif
}