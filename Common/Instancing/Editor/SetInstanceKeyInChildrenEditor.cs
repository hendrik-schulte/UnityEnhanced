#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;

namespace UE.Instancing
{
    
    [CustomEditor(typeof(SetInstanceKeyInChildren), true)]
    [CanEditMultipleObjects]
    public class SetInstanceInChidrenEditor : ReorderableArrayInspector
    {
    }
}
#endif
