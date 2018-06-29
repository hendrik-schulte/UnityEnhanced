using UE.Instancing;

namespace UE.StateMachine
{
    public class EnterState : InstanceObserver
    {
        public State state;
        public State back;

        public void Enter(bool enter)
        {
            if (enter) state.Enter(Key);
            else back.Enter(Key);
        }

        public override IInstanciable Target => !state ? null : state.stateManager;
    }
}