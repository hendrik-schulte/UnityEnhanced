using System;
using UnityEngine.Events;

namespace UE.StateMachine
{
    /// <summary>
    /// A UnityEvent with two States as parameter.
    /// </summary>
    [Serializable]
    public class StateStateEvent : UnityEvent<State, State>
    {
    }
}