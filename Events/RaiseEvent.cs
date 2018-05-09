using UE.Instancing;

namespace UE.Events
{
    public class RaiseEvent : InstanceObserver
    {
        public GameEvent gameEvent;

        public void Raise()
        {
            gameEvent.Raise(key);
        }

        public override IInstanciable GetTarget()
        {
            return gameEvent;
        }
    }
}