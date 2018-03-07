using System;
using UnityEngine;

namespace Variables
{
    [Serializable]
    public class TransformReference : Reference<Transform>
    {
        public TransformVariable Variable;
        
        public override Variable<Transform> GetVariable()
        {
            return Variable;
        }
    }
}