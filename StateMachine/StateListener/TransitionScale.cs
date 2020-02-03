using UE.Common;
using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This component handles nice fade-in fade-out transitions based on the scale of this object.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Transition Scale", 6)]
    public class TransitionScale : TransitionGeneric
    {
        public Vector3 closedScale = Vector3.zero;
        public Vector3 openedScale = Vector3.one;
        
        protected override bool SkipFadingIn => openedScale.AlmostEquals(transform.localScale, 0.01f);

        protected override float AnimationProgress
        {
            set => transform.localScale = Vector3.Lerp(closedScale, openedScale, value);
        }
    }
}