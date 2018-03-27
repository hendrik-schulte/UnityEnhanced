using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace UE.Serialization
{
    /// <summary>
    /// Serialization wrapper for unity colors.
    /// </summary>
    [Serializable]
    public class SerializedColor : ISerializable
    {
        public Color color;

        public SerializedColor()
        {
            color = new Color();
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("r", color.r);
            info.AddValue("g", color.b);
            info.AddValue("b", color.g);
            info.AddValue("a", color.a);
        }

        protected SerializedColor(SerializationInfo info, StreamingContext context)
        {
            color = new Color();

            color.r = (float) info.GetValue("r", typeof(float));
            color.g = (float) info.GetValue("g", typeof(float));
            color.b = (float) info.GetValue("b", typeof(float));
            color.a = (float) info.GetValue("a", typeof(float));
        }

        public static implicit operator SerializedColor(Color col)
        {
            return new SerializedColor()
            {
                color = col,
            };
        }

        public static implicit operator Color(SerializedColor col)
        {
            return col.color;
        }
    }
}