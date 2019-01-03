using System.Collections;
using UE.Variables;
using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This component activates this game object when one of the given states
    /// is activated and disables it as soon as the state is left. There is a
    /// delay before the object is disabled for animations to finish beforehand.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Active In State Delayed", 1)]
    public class ActiveInStateDelayed : StateListener
    {
        [SerializeField] private FloatReference transitionDuration = new FloatReference(1f);
        
        private Coroutine fadeOut;

        protected override void Activated()
        {
            StopRunning();
         
            gameObject.SetActive(true);
        }

        protected override void Deactivated(bool atStart = false)
        {
            if (atStart)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StopRunning();
                fadeOut = StartCoroutine(FadeOut());
            }
        }

        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(transitionDuration);
            
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Stops all currently active animations.
        /// </summary>
        private void StopRunning()
        {
            if (fadeOut != null) StopCoroutine(fadeOut);
        }
    }
}