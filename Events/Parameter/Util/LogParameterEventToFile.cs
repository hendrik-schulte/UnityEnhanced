using UE.Common;
using UE.Instancing;
using UnityEngine;

namespace UE.Events
{
    public abstract class LogParameterEventToFile<T, TS> : InstanceObserver
        where TS : ParameterEvent<T, TS>
    {
        public ParameterEvent<T, TS> paramEvent;

        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        [SerializeField] protected string fileName= "main.log";

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Adding Listener", Logging.Level.Verbose, loggingLevel);
#endif

            paramEvent.AddListener(Triggered, key);
        }

        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Removing Listener", Logging.Level.Verbose, loggingLevel);
#endif

            paramEvent.RemoveListener(Triggered, key);
            
            FileLogger.Close(fileName);
        }

        private void Triggered(T value)
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Writing " + paramEvent.name + " with value " + value + " ...", Logging.Level.Info,
                loggingLevel);
#endif

            FileLogger.Write(fileName, paramEvent.name + " triggered with " + value + ".");
        }

        public override IInstanciable GetTarget()
        {
            return paramEvent;
        }
    }
}