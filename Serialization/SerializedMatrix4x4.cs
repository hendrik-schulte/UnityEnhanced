using System;
using System.Runtime.Serialization;
using UE.Math;
using UnityEngine;

namespace UE.Serialization
{
    /// <summary>
    /// Serialization wrapper for unity Matrix4x4.
    /// </summary>
    [Serializable]
    public class SerializedMatrix4x4 : ISerializable
    {
        public Matrix4x4 m;

        public SerializedMatrix4x4()
        {
            m = new Matrix4x4();
        }

        public SerializedMatrix4x4(Matrix4x4 mat)
        {
            m = mat;
        }

        public SerializedMatrix4x4(Matrix3x3 mat)
        {
            m = mat.ToMatrix4X4();
        }

        public static explicit operator SerializedMatrix4x4(Matrix4x4 mat)
        {
            return new SerializedMatrix4x4(mat);
        }

        public static explicit operator SerializedMatrix4x4(Matrix3x3 mat)
        {
            return new SerializedMatrix4x4(mat);
        }

        public static explicit operator Matrix4x4(SerializedMatrix4x4 mat)
        {
            return mat.m;
        }

        public static explicit operator Matrix3x3(SerializedMatrix4x4 mat)
        {
            return new Matrix3x3(mat.m);
        }


        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("00", m.m00);
            info.AddValue("01", m.m01);
            info.AddValue("02", m.m02);
            info.AddValue("03", m.m03);
            info.AddValue("10", m.m10);
            info.AddValue("11", m.m11);
            info.AddValue("12", m.m12);
            info.AddValue("13", m.m13);
            info.AddValue("20", m.m20);
            info.AddValue("21", m.m21);
            info.AddValue("22", m.m22);
            info.AddValue("23", m.m23);
            info.AddValue("30", m.m30);
            info.AddValue("31", m.m31);
            info.AddValue("32", m.m32);
            info.AddValue("33", m.m33);
        }

        protected SerializedMatrix4x4(SerializationInfo info, StreamingContext context)
        {
            m = new Matrix4x4
            {
                m00 = (float) info.GetValue("00", typeof(float)),
                m01 = (float) info.GetValue("01", typeof(float)),
                m02 = (float) info.GetValue("02", typeof(float)),
                m03 = (float) info.GetValue("03", typeof(float)),
                m10 = (float) info.GetValue("10", typeof(float)),
                m11 = (float) info.GetValue("11", typeof(float)),
                m12 = (float) info.GetValue("12", typeof(float)),
                m13 = (float) info.GetValue("13", typeof(float)),
                m20 = (float) info.GetValue("20", typeof(float)),
                m21 = (float) info.GetValue("21", typeof(float)),
                m22 = (float) info.GetValue("22", typeof(float)),
                m23 = (float) info.GetValue("23", typeof(float)),
                m30 = (float) info.GetValue("30", typeof(float)),
                m31 = (float) info.GetValue("31", typeof(float)),
                m32 = (float) info.GetValue("32", typeof(float)),
                m33 = (float) info.GetValue("33", typeof(float))
            };

        }
    }
}