using System;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace UE.Events
{
    [Serializable]
    public class ObjectUnityEvent : UnityEvent<Object>
    {
    }
}