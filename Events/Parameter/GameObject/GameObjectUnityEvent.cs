using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [Serializable]
    public class GameObjectUnityEvent : UnityEvent<GameObject>
    {
    }
}