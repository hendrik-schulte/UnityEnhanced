#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedTransformEvent))]
    public class InstancedTransformEventDrawer : InstancedParameterEventDrawer<Transform, TransformEvent>
    {
    }
}
#endif
