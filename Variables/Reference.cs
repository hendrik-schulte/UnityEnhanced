﻿// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System;

namespace UE.Variables
{
    [Serializable]
    public abstract class Reference<T>
    {
        public bool UseConstant = true;
        public T ConstantValue;
        public abstract Variable<T> GetVariable();
        
        public Reference()
        { }

        public Reference(T value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public T Value
        {
            get { return UseConstant ? ConstantValue : GetVariable().Value; }
        }

        public static implicit operator T(Reference<T> reference)
        {
            return reference.Value;
        }
    }
}