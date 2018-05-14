#if UNITY_EDITOR
using UnityEditor;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(TransformReference))]
    public class TransformReferenceDrawer : ReferenceDrawer
    {
    }
}
#endif