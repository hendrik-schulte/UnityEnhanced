#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedObjectEvent))]
    public class InstancedObjectEventDrawer : InstancedParameterEventDrawer<Object, ObjectEvent>
    {
    }
}
#endif
