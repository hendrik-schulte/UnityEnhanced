using System;

namespace Variables
{
    [Serializable]
    public class BoolReference : Reference<bool>
    {
        public BoolVariable Variable;
        
        public override Variable<bool> GetVariable()
        {
            return Variable;
        }
    }
}