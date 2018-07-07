using System;
using UnityEngine;

namespace UE.Events
{
    [Serializable]
    public class InstancedGameObjectEvent : InstancedParameterEvent<GameObject, GameObjectEvent>
    {
    }
}