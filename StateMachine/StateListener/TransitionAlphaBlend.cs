using System.Collections;
using UE.Common;
using UE.Variables;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// This component handles nice fade-in fade-out transitions for UI windows. Requires a canvas and
    /// canvas group component for blending.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup), typeof(Canvas))]
    public class TransitionAlphaBlend : StateListener
    {
        [SerializeField] private FloatReference transitionDuration = new FloatReference(1f);

        [Tooltip("When set, this deactivates the the GameObject and not only the canvas.")] [SerializeField]
        private bool disableGameObject;

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
         
            canvas.enabled = true;
            if (disableGameObject) gameObject.SetActive(true);
            
            fadeIn = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            var time = 0f;

            while (time < transitionDuration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = time / transitionDuration;

                yield return null;
            }

            canvasGroup.alpha = 1;
//            Enable();
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
//            Enable();
//            canvasGroup.alpha = 1;
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

            canvas.enabled = false;
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