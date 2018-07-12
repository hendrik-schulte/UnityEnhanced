using System.Collections;
using UE.Common;
using UE.Variables;
using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This <see cref="T:UE.StateMachine.Transition" /> moves to the next state after a given time has passed.
    /// </summary>
    public class TimedTransition : Transition
    {
        [Variables.Range(0, 60)]
        public FloatReference delay;
        
        /// <summary>
        /// Rewinds this transitions progress.
        /// </summary>
        public void Rewind()
        {            
            if(!gameObject.activeInHierarchy) return;
            
            if (!transitState.IsActive(Key))
            {
                transitState.Enter(Key);
                return;
            }
            
#if UNITY_EDITOR
            Logging.Log(this, "Restarting transition ...", Logging.Level.Info, loggingLevel);
#endif
            
            StartTransition(false);
        }
        
        protected override IEnumerator TransitionStart()
        {
            yield return new WaitForSeconds(delay);
            
            TransitionComplete();
        }
    }
}