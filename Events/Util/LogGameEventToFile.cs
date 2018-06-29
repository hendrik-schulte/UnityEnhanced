using UE.Common;
using UE.Instancing;
using UnityEngine;

namespace UE.Events
{
    public class LogGameEventToFile : InstanceObserver
    {
        public GameEvent gameEvent;

        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        [SerializeField] protected string fileName= "main.log";

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Adding Listener", Logging.Level.Verbose, loggingLevel);
#endif

            gameEvent.AddListener(Triggered, Key);
        }

        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Removing Listener", Logging.Level.Verbose, loggingLevel);
#endif

            gameEvent.RemoveListener(Triggered, Key);
            
            FileLogger.Close(fileName);
        }

        private void Triggered()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Writing " + gameEvent.name + " ...", Logging.Level.Info,
                loggingLevel);
#endif

            FileLogger.Write(fileName, gameEvent.name + " triggered.");
        }

        public override IInstanciable Target => gameEvent;
    }
}