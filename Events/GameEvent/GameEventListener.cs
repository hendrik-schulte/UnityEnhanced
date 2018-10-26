using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    /// <summary>
    /// A component that observes a given event and forwards it to a UnityEvent.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Events/Game Event Listener", 0)]
    public class GameEventListener : InstanceObserver
    {
        [Tooltip("When this is checked, the listener will still work when the game object is disabled. However the " +
                 "GameObject needs to be activated at least once for the Awake or OnEnable hooks to be invoked.")]
        [SerializeField]
        private bool persistent;
        
        [Tooltip("Event to register with.")] [SerializeField]
        internal GameEvent Event;
        
        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;
        
        /// <summary>
        /// Override this to disable the UnityEvent in the inspector.
        /// </summary>
        public virtual bool DrawUnityEventInspector => true;

        protected virtual void OnEnable()
        {
            if (!persistent) Event.AddListener(this, Key);
        }

        protected virtual void OnDisable()
        {
            if (!persistent) Event.RemoveListener(this, Key);
        }

        protected virtual void Awake()
        {
            if (persistent) Event.AddListener(this, Key);
        }

        protected virtual void OnDestroy()
        {
            if (persistent) Event.RemoveListener(this, Key);
        }

        /// <summary>
        /// This is called when the listened event gets raised.
        /// </summary>
        public virtual void OnEventRaised()
        {
            Response.Invoke();
        }
        
        /// <summary>
        /// Replaces the current event by the given one.
        /// </summary>
        /// <param name="gameEvent"></param>
        public virtual void SetEvent(GameEvent gameEvent)
        {
            if (Application.isPlaying)
            {
                if(Event != null) Event.RemoveListener(this);
                gameEvent.AddListener(this);                
            }
            
            Event = gameEvent;
        }

        public override IInstanciable Target => Event;
    }
}