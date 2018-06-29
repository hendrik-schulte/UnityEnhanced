using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedVector3Event : InstancedParameterEvent<Vector3, Vector3Event>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedVector3Event))]
    public class InstancedVector3EventDrawer : InstancedParameterEventDrawer<Vector3, Vector3Event>
    {
    }
#endif
}