using System.Collections;
using UE.Common;
using UE.Variables;
using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This component handles nice fade-in fade-out transitions for UI windows. Requires a
    /// canvas group component for blending.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Transition Alpha Blend", 3)]
    [RequireComponent(typeof(CanvasGroup))]
    public class TransitionAlphaBlend : StateListener
    {
        [SerializeField] private FloatReference transitionDuration = new FloatReference(1f);

        [Tooltip("When set, this deactivates the the GameObject and not only the canvas.")] [SerializeField]
        private bool disableGameObject;
        
        [Tooltip("Wait for fading in until other UI has faded out.")] [SerializeField]
        private bool delayedFadeIn;

        private CanvasGroup canvasGroup;
        private Canvas canvas;

        private Coroutine fadeIn, fadeOut;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();

            if (transitionDuration <= 0)
            {
                Logging.Warning(this, "The duration of this transition is zero or negative!");
            }
        }

        protected override void Activated()
        {
            StopRunning();
         
            if(canvas) canvas.enabled = true;
            if (disableGameObject) gameObject.SetActive(true);
            
            fadeIn = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            if(canvasGroup.alpha > 0.99) yield break;
         
            if(delayedFadeIn) yield return new WaitForSeconds(transitionDuration);
            
            var time = 0f;

            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = time / transitionDuration;

                yield return null;
            }

            canvasGroup.alpha = 1;
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
                canvasGroup.alpha = 1 - time / transitionDuration;

                yield return null;
            }
            
            Disable();
        }

        private void Disable()
        {
            canvasGroup.alpha = 0f;

            if(canvas) canvas.enabled = false;
            if (disableGameObject) gameObject.SetActive(false);
        }

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