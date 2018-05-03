using System;
using UnityEngine.Events;

namespace UE.StateMachine
{
    /// <summary>
    /// A UnityEvent with a State as parameter.
    /// </summary>
    [Serializable]
    public class StateEvent : UnityEvent<State>
    {
    }
}