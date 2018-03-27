using UnityEngine;

namespace UE.Events
{
    public class RaiseEvent : MonoBehaviour
    {
        public GameEvent gameEvent;

        public void Raise()
        {
            gameEvent.Raise();
        }
    }
}