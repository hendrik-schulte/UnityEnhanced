using System;
using System.Runtime.Serialization;
using UE.Math;
using UnityEngine;

namespace UE.Serialization
{
    public static class SerializationExtensions
    {
        /// <summary>
        /// Tries to serialize the given value. Use GetValueGeneric to deserialize it. Use a unique prefix to serialize multiple values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="value"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static bool AddValueGeneric<T>(this SerializationInfo info, T value, string prefix = "")
        {
            if (typeof(T) == typeof(Vector2))
            {
                var vec2 = (Vector2) Convert.ChangeType(value, typeof(Vector2));

                info.AddValue(prefix + "x", vec2.x);
                info.AddValue(prefix + "y", vec2.y);

                return true;
            }

            if (typeof(T) == typeof(Vector3))
            {
                var vec3 = (Vector3) Convert.ChangeType(value, typeof(Vector3));

                info.AddValue(prefix + "x", vec3.x);
                info.AddValue(prefix + "y", vec3.y);
                info.AddValue(prefix + "z", vec3.z);

                return true;
            }

            if (typeof(T) == typeof(Vector4))
            {
                var vec3 = (Vector4) Convert.ChangeType(value, typeof(Vector4));

                info.AddValue(prefix + "x", vec3.x);
                info.AddValue(prefix + "y", vec3.y);
                info.AddValue(prefix + "z", vec3.z);
                info.AddValue(prefix + "w", vec3.w);

                return true;
            }

            if (typeof(T) == typeof(Matrix4x4))
            {
                var serVal = (SerializedMatrix4x4) (Matrix4x4) Convert.ChangeType(value, typeof(Matrix4x4));

                info.AddValue(prefix + "value", serVal);

                return true;
            }

            if (typeof(T) == typeof(Matrix3x3))
            {
                var serVal = (SerializedMatrix4x4) (Matrix3x3) Convert.ChangeType(value, typeof(Matrix3x3));

                info.AddValue(prefix + "value", serVal);

                return true;
            }

            info.AddValue(prefix + "value", value);

            if (!(typeof(T).IsSerializable || typeof(T) == typeof(float) || typeof(T) == typeof(int)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Tries to deserialize the generic type. Tha value should be serialized by AddValueGeneric. Use a unique prefix to serialize multiple values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static T GetValueGeneric<T>(this SerializationInfo info, string prefix = "")
        {
            if (typeof(T) == typeof(Vector2))
            {
                return (T)Convert.ChangeType(new Vector2
                {
                    x = (float)info.GetValue(prefix + "x", typeof(float)),
                    y = (float)info.GetValue(prefix + "y", typeof(float))

                }, typeof(T));
            }

            if (typeof(T) == typeof(Vector3))
            {
                return (T)Convert.ChangeType(new Vector3
                {
                    x = (float)info.GetValue(prefix + "x", typeof(float)),
                    y = (float)info.GetValue(prefix + "y", typeof(float)),
                    z = (float)info.GetValue(prefix + "z", typeof(float))

                }, typeof(T));
            }

            if (typeof(T) == typeof(Vector4))
            {
                return (T)Convert.ChangeType(new Vector4
                {
                    x = (float)info.GetValue(prefix + "x", typeof(float)),
                    y = (float)info.GetValue(prefix + "y", typeof(float)),
                    z = (float)info.GetValue(prefix + "z", typeof(float)),
                    w = (float)info.GetValue(prefix + "w", typeof(float))

                }, typeof(T));
            }

            if (typeof(T) == typeof(Matrix3x3))
            {
                return (T)Convert.ChangeType((Matrix3x3)(SerializedMatrix4x4)info.GetValue(prefix + "value", typeof(SerializedMatrix4x4)), typeof(T));
            }

            if (typeof(T) == typeof(Matrix4x4))
            {
                return (T)Convert.ChangeType((Matrix4x4)(SerializedMatrix4x4)info.GetValue(prefix + "value", typeof(SerializedMatrix4x4)), typeof(T));
            }

            if (!(typeof(T).IsSerializable || typeof(T) == typeof(float) || typeof(T) == typeof(int)))
            {
                Debug.LogWarning("Deserializing a constant node. This fails if the type isnt serializable");
            }

            return (T)info.GetValue(prefix + "value", typeof(T));
        }
    }
}