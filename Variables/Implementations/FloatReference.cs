using System;

namespace UE.Variables
{
    [Serializable]
    public class FloatReference : Reference<float>
    {
        public FloatVariable Variable;

        public FloatReference()
        {
            
        }
        
        public FloatReference(float value) : base(value)
        {
        }
        
        public override Variable<float> GetVariable()
        {
            return Variable;
        }
    }
}