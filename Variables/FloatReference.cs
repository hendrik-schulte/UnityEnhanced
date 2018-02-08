using System;

namespace Variables
{
    [Serializable]
    public class FloatReference : Reference<float>
    {
        public FloatVariable Variable;
        
        public override Variable<float> GetVariable()
        {
            return Variable;
        }
    }
}