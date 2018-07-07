#if UNITY_EDITOR

using UnityEditor;

namespace UE.Events
{
    [CustomEditor(typeof(IntEvent), true)]
    [CanEditMultipleObjects]
    public class IntEventEditor : ParameterEventEditor<int, IntEvent>
    {
    }
}
#endif