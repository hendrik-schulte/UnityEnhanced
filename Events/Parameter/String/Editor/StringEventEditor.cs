#if UNITY_EDITOR

using UnityEditor;

namespace UE.Events
{
    [CustomEditor(typeof(StringEvent), true)]
    [CanEditMultipleObjects]
    public class StringEventEditor : ParameterEventEditor<string, StringEvent>
    {
    }
}
#endif