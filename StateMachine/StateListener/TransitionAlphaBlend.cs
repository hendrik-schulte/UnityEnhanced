using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This component handles nice fade-in fade-out transitions for UI windows. Requires a
    /// canvas group component for blending.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Transition Alpha Blend", 4)]
    [RequireComponent(typeof(CanvasGroup))]
    public class TransitionAlphaBlend : TransitionGeneric
    {
        private CanvasGroup canvasGroup;
        private Canvas canvas;

        private bool skipFadingIn;
        private float value;

        protected override void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvas = GetComponent<Canvas>();
        }

        protected override void Activated()
        {
            if(canvas) canvas.enabled = true;
            
            base.Activated();
        }

        protected override void Disable()
        {
            if(canvas) canvas.enabled = false;
            
            base.Disable();
        }

        protected override bool SkipFadingIn => canvasGroup.alpha > 0.99;

        protected override float AnimationProgress
        {
            set => canvasGroup.alpha = value;
        }
    }
}