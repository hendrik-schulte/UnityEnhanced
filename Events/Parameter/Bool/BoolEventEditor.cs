#if UNITY_EDITOR

using UnityEditor;

namespace UE.Events
{
    [CustomEditor(typeof(BoolEvent), true)]
    [CanEditMultipleObjects]
    public class BoolEventEditor : ParameterEventEditor<bool, BoolEvent>
    {
    }
}
#endif