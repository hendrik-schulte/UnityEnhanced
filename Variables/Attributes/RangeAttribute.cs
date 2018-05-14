using UnityEngine;

namespace UE.Variables
{
    /// <summary>
    /// Use this version of the [Range] attribute for FloatReference or IntReference fields
    /// as you would normally use UnityEngine.RangeAttribute.
    /// </summary>
    public class RangeAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public RangeAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}