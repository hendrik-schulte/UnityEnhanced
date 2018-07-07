#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedGameObjectEvent))]
    public class InstancedGameObjectEventDrawer : InstancedParameterEventDrawer<GameObject, GameObjectEvent>
    {
    }
}
#endif
