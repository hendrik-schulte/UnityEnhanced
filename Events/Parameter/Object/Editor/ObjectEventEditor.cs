#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(ObjectEvent), true)]
    [CanEditMultipleObjects]
    public class ObjectEventEditor : ParameterEventEditor<Object, ObjectEvent>
    {
    }
}
#endif