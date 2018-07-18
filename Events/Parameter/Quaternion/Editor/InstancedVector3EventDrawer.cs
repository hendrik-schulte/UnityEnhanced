#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedQuaternionEvent))]
    public class InstancedQuaternionEventDrawer : InstancedParameterEventDrawer<Quaternion, QuaternionEvent>
    {
    }
}
#endif
