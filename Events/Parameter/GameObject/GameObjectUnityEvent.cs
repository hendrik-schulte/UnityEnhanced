using System;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Events
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject>
    {
    }
}