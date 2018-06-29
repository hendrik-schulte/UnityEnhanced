using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedVector2Event : InstancedParameterEvent<Vector2, Vector2Event>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedVector2Event))]
    public class InstancedVector2EventDrawer : InstancedParameterEventDrawer<Vector2, Vector2Event>
    {
    }
#endif
}