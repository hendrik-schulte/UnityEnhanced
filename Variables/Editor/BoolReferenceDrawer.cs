#if UNITY_EDITOR
using UnityEditor;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolReferenceDrawer : ReferenceDrawer
    {
    }
}
#endif