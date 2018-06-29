using UE.Instancing;

namespace UE.Events
{
    public class RaiseEvent : InstanceObserver
    {
        public GameEvent gameEvent;

        public void Raise()
        {
            gameEvent.Raise(Key);
        }

        public override IInstanciable Target => gameEvent;
    }
}

