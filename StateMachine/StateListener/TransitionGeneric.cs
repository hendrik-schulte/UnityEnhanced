using System.Collections;
using UE.Common;
using UE.Events;
using UE.Variables;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// This component extends the StateListener by generic animated behaviour. Can for example be used to animate the
    /// intensity of a light source.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Transition Generic", 3)]
    public class TransitionGeneric : StateListener
    {
        [SerializeField] private FloatReference transitionDuration = new FloatReference(1f);

        [Tooltip("When set, this deactivates the the GameObject and not only the canvas.")] [SerializeField]
        private bool disableGameObject;
        
        [Tooltip("Delays the fadeIn animation by the transition duration to ensure previous elements have " +
                 "been faded out before this fades in.")] [SerializeField]
        private bool delayedFadeIn;

        
        [Tooltip("Allows to define a custom mapping of the animation progress (from 0 to 1) to the actual.")]
        [SerializeField] private AnimationCurve curveMapping = AnimationCurve.Linear(0,0,1,1);
        [SerializeField] private FloatUnityEvent OnAnimate;
        
        private Coroutine fadeIn, fadeOut;

        protected virtual void Awake()
        {
            if (transitionDuration <= 0)
            {
                Logging.Warning(this, "The duration of this transition is zero or negative!");
            }
        }

        protected override void Activated()
        {
            StopRunning();
         
            if (disableGameObject) gameObject.SetActive(true);
            
            fadeIn = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            if(SkipFadingIn) yield break;
            
            if(delayedFadeIn) yield return new WaitForSeconds(transitionDuration);
            
            var time = 0f;

            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                ValueInternal = time / transitionDuration;

                yield return null;
            }

            ValueInternal = 1;
        }

        protected override void Deactivated(bool atStart = false)
        {
            if (atStart)
            {
                Disable();
                return;
            }
            
            StopRunning();
            fadeOut = StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            var time = 0f;

            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                ValueInternal = 1 - time / transitionDuration;

                yield return null;
            }
            
            Disable();
        }

        protected virtual void Disable()
        {
            ValueInternal = 0;

            if (disableGameObject) gameObject.SetActive(false);
        }

        private float ValueInternal
        {
            set
            {
                var valueAfterCurve = curveMapping.Evaluate(value);
                
                OnAnimate.Invoke(valueAfterCurve);
                AnimationProgress = valueAfterCurve;
            }
        }

        /// <summary>
        /// Override this to animate custom behavior.
        /// </summary>
        // ReSharper disable once ValueParameterNotUsed
        protected virtual float AnimationProgress { set{} }

        /// <summary>
        /// Override this to skip fading in when the target value is already at 100%.v
        /// </summary>
        protected virtual bool SkipFadingIn => false;

        /// <summary>
        /// Stops all currently active animations.
        /// </summary>
        private void StopRunning()
        {
            if (fadeIn != null) StopCoroutine(fadeIn);
            if (fadeOut != null) StopCoroutine(fadeOut);
        }
    }
}