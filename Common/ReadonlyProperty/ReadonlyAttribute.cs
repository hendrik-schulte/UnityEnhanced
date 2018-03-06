using UnityEngine;

namespace ReadonlyProperty
{
    public class ReadonlyAttribute : PropertyAttribute
    {    
        /// <summary>
        /// Using this attribute on a serialized property in Unity will make it read only in the inspector.
        /// </summary>
        public ReadonlyAttribute()
        {
        }
    }
}