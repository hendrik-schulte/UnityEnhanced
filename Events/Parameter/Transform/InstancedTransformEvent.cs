using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedTransformEvent : InstancedParameterEvent<Transform, TransformEvent>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedTransformEvent))]
    public class InstancedTransformEventDrawer : InstancedParameterEventDrawer<Transform, TransformEvent>
    {
    }
#endif
}