// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("When this is checked, the listener will still work when the game object is disabled.")]
        public bool persistent;
        
        [Tooltip("Event to register with.")] public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        private void OnEnable()
        {
            if (!persistent) Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (!persistent) Event.UnregisterListener(this);
        }

        private void Awake()
        {
            if (persistent) Event.RegisterListener(this);
        }

        private void OnDestroy()
        {
            if (persistent) Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}