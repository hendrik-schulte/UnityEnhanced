using System;

namespace UE.Variables
{
    [Serializable]
    public class BoolReference : Reference<bool>
    {
        public BoolVariable Variable;

        public BoolReference()
        {
        }

        public BoolReference(bool value) : base(value)
        {
        }

        public override Variable<bool> GetVariable()
        {
            return Variable;
        }
    }
}