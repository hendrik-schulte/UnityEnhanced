﻿using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Unity Enhanced/Events/Event(Vector3)")]
    public class Vector3Event : ParameterEvent<Vector3, Vector3Event>
    {
        [SerializeField]
        private Vector3UnityEvent OnTriggered = new Vector3UnityEvent();

        protected override UnityEvent<Vector3> OnEventTriggered => OnTriggered;
        
#if UE_Photon
        public override bool IsNetworkingType => true;
#endif
    }
}