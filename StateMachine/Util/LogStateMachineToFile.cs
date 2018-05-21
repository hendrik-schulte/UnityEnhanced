using UE.Common;
using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    public class LogStateMachineToFile : InstanceObserver
    {
        public StateManager stateManager;

        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;
        
        [SerializeField] protected string fileName = "main.log";

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Adding Listener", Logging.Level.Verbose, loggingLevel);
#endif
            
            stateManager.AddStateEnterListener(OnStateEnter, key);
        }
        
        protected virtual void OnDestroy()
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Removing Listener", Logging.Level.Verbose, loggingLevel);
#endif

            stateManager.RemoveStateEnterListener(OnStateEnter, key);
        }
        
        private void OnStateEnter(State state)
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Writing ...", Logging.Level.Info, loggingLevel);
#endif

            FileLogger.Write(fileName, state.name + " was entered.");
        }
        
        public override IInstanciable GetTarget()
        {
            return stateManager;
        }
    }
}