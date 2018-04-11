#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(Vector3Event), true)]
    [CanEditMultipleObjects]
    public class Vector3EventEditor : ParameterEventEditor<Vector3, Vector3Event>
    {
    }
}
#endif