#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(QuaternionEvent), true)]
    [CanEditMultipleObjects]
    public class QuaternionEventEditor : ParameterEventEditor<Quaternion, QuaternionEvent>
    {
    }
}
#endif