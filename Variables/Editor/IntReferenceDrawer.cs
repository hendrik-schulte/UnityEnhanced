#if UNITY_EDITOR
using UnityEditor;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer : ReferenceDrawer
    {
    }
}
#endif