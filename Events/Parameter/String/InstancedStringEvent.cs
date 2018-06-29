using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedStringEvent : InstancedParameterEvent<string, StringEvent>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedStringEvent))]
    public class InstancedStringEventDrawer : InstancedParameterEventDrawer<string, StringEvent>
    {
    }
#endif
}