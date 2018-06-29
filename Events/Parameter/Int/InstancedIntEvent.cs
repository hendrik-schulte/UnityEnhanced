using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedIntEvent : InstancedParameterEvent<int, IntEvent>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedIntEvent))]
    public class InstancedIntEventDrawer : InstancedParameterEventDrawer<int, IntEvent>
    {
    }
#endif
}