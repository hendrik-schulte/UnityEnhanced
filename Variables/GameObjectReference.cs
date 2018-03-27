using System;
using UnityEngine;

namespace UE.Variables
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