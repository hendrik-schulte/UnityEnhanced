// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace UE.Variables
{
    /// <summary>
    /// This is a Variable wrapped in a ScriptableObject. You can reference Variables directly from a MonoBehaviour
    /// or you use a <see cref="Reference{T}"/> so you can decide use a constant value instead the inspector.
    /// Needs to be implemented for concrete types!
    /// </summary>
    /// <typeparam name="T">The Type to be saved</typeparam>
    public class Variable<T> : ScriptableObject, ISerializationCallbackReceiver
    {
        //Removed compiler directive because of errors in build (Unexpected serialization layout). 
        [Multiline]
        public string DeveloperDescription = "";

        /// <summary>
        /// The value of this variable. Changing this will be persistent after leaving play mode.
        /// </summary>
        public T Value;
        
        /// <summary>
        /// The value of this variable is being set to <see cref="Value"/> when entering play mode and is not persistent
        /// after leaving play mode.
        /// </summary>
        [NonSerialized]
        public T RuntimeValue;

        public void SetValue(T value)
        {
            Value = value;
        }

        public void SetValue(Variable<T> value)
        {
            Value = value.Value;
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            RuntimeValue = Value;
        }
    }
}