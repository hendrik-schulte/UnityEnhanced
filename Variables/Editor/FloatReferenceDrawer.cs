#if UNITY_EDITOR
using UnityEditor;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer : ReferenceDrawer
    {
    }
}
#endif