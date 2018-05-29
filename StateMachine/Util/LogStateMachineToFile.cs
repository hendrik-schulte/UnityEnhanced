using System.Globalization;
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

        [SerializeField] protected bool logPassedTime = false;
        private float timePassed = 0;

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
            Logging.Log(this, "'" + gameObject.name + "' Removing Listener and closing stream.", Logging.Level.Verbose,
                loggingLevel);
#endif

            stateManager.RemoveStateEnterListener(OnStateEnter, key);

            FileLogger.Close(fileName);
        }

        private void OnStateEnter(State state)
        {
#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Writing ...", Logging.Level.Info, loggingLevel);
#endif

            if (logPassedTime)
            {
                FileLogger.Write(fileName, ", " + timePassed.ToString("F2", CultureInfo.InvariantCulture) + ", " + state.name + " was entered.");
                
                timePassed = 0;
            }
            else FileLogger.Write(fileName, state.name + " was entered.");
        }

        public override IInstanciable GetTarget()
        {
            return stateManager;
        }

        private void Update()
        {
            timePassed += Time.deltaTime;
        }
    }
}