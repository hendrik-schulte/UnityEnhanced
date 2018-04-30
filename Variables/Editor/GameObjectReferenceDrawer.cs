#if UNITY_EDITOR
using UnityEditor;

namespace UE.Variables
{
    [CustomPropertyDrawer(typeof(GameObjectReference))]
    public class GameObjectReferenceDrawer : ReferenceDrawer
    {
    }
}
#endif