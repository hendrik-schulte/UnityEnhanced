// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using UnityEngine;

namespace UE.Variables
{
    public class Variable<T> : ScriptableObject
    {
        //Removed compiler directive because of errors in build (Unexpected serialization layout). 
        [Multiline]
        public string DeveloperDescription = "";

        public T Value;

        public void SetValue(T value)
        {
            Value = value;
        }

        public void SetValue(Variable<T> value)
        {
            Value = value.Value;
        }
    }
}