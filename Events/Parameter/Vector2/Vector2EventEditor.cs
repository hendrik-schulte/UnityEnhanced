#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(Vector2Event), true)]
    [CanEditMultipleObjects]
    public class Vector2EventEditor : ParameterEventEditor<Vector2, Vector2Event>
    {
    }
}
#endif