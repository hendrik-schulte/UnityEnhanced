#if UNITY_EDITOR

using UnityEditor;

namespace UE.Events
{
    [CustomEditor(typeof(FloatEvent), true)]
    [CanEditMultipleObjects]
    public class FloatEventEditor : ParameterEventEditor<float, FloatEvent>
    {
    }
}
#endif