using UE.Common;
using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
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