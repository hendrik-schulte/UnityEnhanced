﻿using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This component deactivates this game object when one of the given states
    /// is activated and activates it as soon as the state is left.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Inactive In State", 2)]
    public class InactiveInState : StateListener
    {
        protected override void Activated()
        {
            gameObject.SetActive(false);
        }

        protected override void Deactivated(bool atStart = false)
        {
            gameObject.SetActive(true);
        }
    }
}