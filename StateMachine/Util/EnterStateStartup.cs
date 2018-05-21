using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// This enters the target state at start.
    /// Consider using the initial state of the state machine before using this.
    /// </summary>
    public class EnterStateStartup : InstanceObserver
    {
        [SerializeField]
        private State state;

        private void Start()
        {
            state.Enter(key);
        }

        public override IInstanciable GetTarget()
        {
            return !state ? null : state.stateManager;
        }
    }
}