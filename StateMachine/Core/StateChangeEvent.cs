using System;
using UnityEngine.Events;

namespace StateMachine
{
    [Serializable]
    public class StateChangeEvent : UnityEvent<State>
    {
    }
}