using System;

namespace UE.Variables
{
    [Serializable]
    public class IntReference : Reference<int>
    {
        public IntVariable Variable;
        
        public IntReference()
        {
        }

        public IntReference(int value) : base(value)
        {
        }

        public override Variable<int> GetVariable()
        {
            return Variable;
        }
    }
}