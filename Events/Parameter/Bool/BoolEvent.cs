﻿using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [CreateAssetMenu(menuName = "Events/Event(string)")]
    public class BoolEvent : ParameterEvent<bool>
    {
        [SerializeField]
        private BoolUnityEvent OnTriggered;

        protected override UnityEvent<bool> OnEventTriggered => OnTriggered;
    }
}