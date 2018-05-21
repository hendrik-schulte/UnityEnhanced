using UE.Instancing;

namespace UE.StateMachine
{
    public class EnterState : InstanceObserver
    {
        public State state;
        public State back;

        public void Enter(bool enter)
        {
            if (enter) state.Enter(key);
            else back.Enter(key);
        }

        public override IInstanciable GetTarget()
        {
            return !state ? null : state.stateManager;
        }
    }
}