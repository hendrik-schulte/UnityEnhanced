using System;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace UE.Events
{
    [Serializable]
    public class InstancedFloatEvent : InstancedParameterEvent<float, FloatEvent>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedFloatEvent))]
    public class InstancedFloatEventDrawer : InstancedParameterEventDrawer<float, FloatEvent>
    {
    }
#endif
}