#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(TransformEvent), true)]
    [CanEditMultipleObjects]
    public class TransformEventEditor : ParameterEventEditor<Transform, TransformEvent>
    {
    }
}
#endif