using System;
using Object = UnityEngine.Object;

namespace UE.Events
{
    [Serializable]
    public class InstancedObjectEvent : InstancedParameterEvent<Object, ObjectEvent>
    {
    }
}