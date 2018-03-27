using System;
using System.Runtime.Serialization;

namespace UE.Serialization
{
    /// <summary>
    /// Serializable wrapper for a boolean value.
    /// </summary>
    [Serializable]
    public class SerializedBool : ISerializable
    {
        [NonSerialized] public bool boolean;

        public SerializedBool()
        {
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("boolean", boolean);
        }

        private SerializedBool(SerializationInfo info, StreamingContext context)
        {
            boolean = (bool) info.GetValue("boolean", typeof(bool));
        }

        public static implicit operator SerializedBool(bool boolean)
        {
            return new SerializedBool()
            {
                boolean = boolean,
            };
        }

        public static implicit operator bool(SerializedBool boolean)
        {
            return boolean.boolean;
        }
    }
}