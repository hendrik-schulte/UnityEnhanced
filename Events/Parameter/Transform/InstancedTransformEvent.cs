using System;
using UnityEngine;

namespace UE.Events
{
    [Serializable]
    public class InstancedTransformEvent : InstancedParameterEvent<Transform, TransformEvent>
    {
    }
}