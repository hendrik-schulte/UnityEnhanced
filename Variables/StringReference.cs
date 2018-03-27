using System;

namespace UE.Variables
{
    [Serializable]
    public class StringReference : Reference<string>
    {
        public StringVariable Variable;
        
        public override Variable<string> GetVariable()
        {
            return Variable;
        }
    }
}