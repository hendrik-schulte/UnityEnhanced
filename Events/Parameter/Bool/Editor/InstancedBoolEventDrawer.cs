#if UNITY_EDITOR
using UnityEditor;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedBoolEvent))]
    public class InstancedBoolEventDrawer : InstancedParameterEventDrawer<bool, BoolEvent>
    {
    }
}
#endif