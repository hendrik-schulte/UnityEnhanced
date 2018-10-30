// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System;

namespace UE.Variables
{
    /// <summary>
    /// A Reference can either direct to a <see cref="Variable{T}"/> ScriptableObject or a constant value which can be
    /// defined in the inspector. Needs to be implemented for concrete types!
    /// </summary>
    /// <typeparam name="T">The Type to be referenced</typeparam>
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

        /// <summary>
        /// Returns the runtime value of this reference (either constant or reference).
        /// </summary>
        public T Value
        {
            get { return UseConstant ? ConstantValue : GetVariable().RuntimeValue; }
        }
        
        /// <summary>
        /// Returns the editor value of this reference (either constant or reference).
        /// </summary>
        public T StaticValue
        {
            get { return UseConstant ? ConstantValue : GetVariable().Value; }
        }

        public static implicit operator T(Reference<T> reference)
        {
            return reference.Value;
        }
    }
}