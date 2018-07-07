#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedFloatEvent))]
    public class InstancedFloatEventDrawer : InstancedParameterEventDrawer<float, FloatEvent>
    {
    }
}
#endif
