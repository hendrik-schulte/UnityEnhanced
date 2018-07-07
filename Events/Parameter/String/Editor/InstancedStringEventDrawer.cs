#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedStringEvent))]
    public class InstancedStringEventDrawer : InstancedParameterEventDrawer<string, StringEvent>
    {
    }
}
#endif
