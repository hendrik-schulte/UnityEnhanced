// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event()")]
    public class GameEvent : ScriptableObject
    {    
        [SerializeField] 
        private bool debugLog;    
        
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
      
        [SerializeField]
        private UnityEvent OnEventTriggered;

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<GameEventListener> eventListeners = 
            new List<GameEventListener>();

        public void Raise()
        {
            if(debugLog) Debug.Log("Game Event '" + name + "' was raised!");
            
            for(var i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
            OnEventTriggered.Invoke();
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void RegisterListener(UnityAction listener)
        {
            OnEventTriggered.AddListener(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
        
        public void UnregisterListener(UnityAction listener)
        {
            OnEventTriggered.RemoveListener(listener);   
        }
    }
}