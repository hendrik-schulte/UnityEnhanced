#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedVector3Event))]
    public class InstancedVector3EventDrawer : InstancedParameterEventDrawer<Vector3, Vector3Event>
    {
    }
}
#endif
