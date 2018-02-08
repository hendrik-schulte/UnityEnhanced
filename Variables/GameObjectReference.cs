using System;
using UnityEngine;

namespace Variables
{
    [Serializable]
    public class GameObjectReference : Reference<GameObject>
    {
        public GameObjectVariable Variable;
        
        public override Variable<GameObject> GetVariable()
        {
            return Variable;
        }
    }
}