using UnityEditor;

namespace Variables
{
    [CustomPropertyDrawer(typeof(GameObjectReference))]
    public class GameObjectReferenceDrawer : ReferenceDrawer
    {
    }
}