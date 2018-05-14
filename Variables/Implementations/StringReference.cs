using System;

namespace UE.Variables
{
    [Serializable]
    public class StringReference : Reference<string>
    {
        public StringVariable Variable;
        
        public StringReference() : base()
        {
        }

        public StringReference(string value) : base(value)
        {
        }

        public override Variable<string> GetVariable()
        {
            return Variable;
        }
    }
}