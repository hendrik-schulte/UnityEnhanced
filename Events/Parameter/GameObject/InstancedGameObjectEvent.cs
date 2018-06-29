using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Events
{
    [Serializable]
    public class InstancedGameObjectEvent : InstancedParameterEvent<GameObject, GameObjectEvent>
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(InstancedGameObjectEvent))]
    public class InstancedGameObjectEventDrawer : InstancedParameterEventDrawer<GameObject, GameObjectEvent>
    {
    }
#endif
}