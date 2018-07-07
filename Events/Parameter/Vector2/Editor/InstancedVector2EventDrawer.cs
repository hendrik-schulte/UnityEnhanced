#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedVector2Event))]
    public class InstancedVector2EventDrawer : InstancedParameterEventDrawer<Vector2, Vector2Event>
    {
    }
}
#endif
