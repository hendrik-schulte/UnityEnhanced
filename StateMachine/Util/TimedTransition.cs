using System.Collections;
using UE.Common;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// This transition moves to the next state after a given time has passed.
    /// </summary>
    public class TimedTransition : Transition
    {
        [Range(0, 60)]
        public float delay;
        
        /// <summary>
        /// Rewinds this transitions progress.
        /// </summary>
        public void Rewind()
        {            
            if(!gameObject.activeInHierarchy) return;
            
            if (!transitState.IsActive(key))
            {
                transitState.Enter(key);
                return;
            }
            
            Logging.Log(this, "'" + gameObject.name + "' Restarting transition ...", Logging.Level.Info, loggingLevel);

            StartTransition(false);
        }
        
        protected override IEnumerator TransitionStart()
        {
            yield return new WaitForSeconds(delay);
            
            TransitionComplete();
        }
    }
}