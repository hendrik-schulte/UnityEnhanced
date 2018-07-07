#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedIntEvent))]
    public class InstancedIntEventDrawer : InstancedParameterEventDrawer<int, IntEvent>
    {
    }
}
#endif
