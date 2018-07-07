using System;

namespace UE.Events
{
    [Serializable]
    public class InstancedBoolEvent : InstancedParameterEvent<bool, BoolEvent>
    {
        /// <summary>
        /// Raises this event with true as parameter.
        /// </summary>
        public void RaiseTrue()
        {
            paramEvent.Raise(true, Key);
        }
        
        /// <summary>
        /// Raises this event with with false as parameter.
        /// </summary>
        public void RaiseFalse()
        {
            paramEvent.Raise(false);
        }
    }
}