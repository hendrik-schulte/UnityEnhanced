using System.Collections;
using UnityEngine;

namespace UE.StateMachine
{
    public class TimedTransition : Transition
    {
        [Range(0, 60)]
        public float delay;
        
        protected override IEnumerator TransitionStart()
        {
            yield return new WaitForSeconds(delay);
            
            TransitionComplete();
        }
    }
}