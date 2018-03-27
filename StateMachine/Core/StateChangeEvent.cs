using System;
using UnityEngine.Events;

namespace UE.StateMachine
{
    [Serializable]
    public class StateChangeEvent : UnityEvent<State>
    {
    }
}