using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace UE.Serialization
{
    /// <summary>
    /// Serialization wrapper for unity vector3.
    /// </summary>
    [Serializable]
    public class SerializedVector3 : ISerializable
    {
        public Vector3 vector;

        public SerializedVector3()
        {
            vector = new Vector3();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", vector.x);
            info.AddValue("Y", vector.y);
            info.AddValue("Z", vector.z);
        }

        protected SerializedVector3(SerializationInfo info, StreamingContext context)
        {
            vector = new Vector3
            {
                x = (float) info.GetValue("X", typeof(float)),
                y = (float) info.GetValue("Y", typeof(float)),
                z = (float) info.GetValue("Z", typeof(float))
            };

        }
    }
}