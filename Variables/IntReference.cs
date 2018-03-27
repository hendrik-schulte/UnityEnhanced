using System;

namespace UE.Variables
{
    [Serializable]
    public class IntReference : Reference<int>
    {
        public IntVariable Variable;
        
        public override Variable<int> GetVariable()
        {
            return Variable;
        }
    }
}