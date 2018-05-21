using System.Collections;
using UE.Common;
using UnityEngine;

namespace UE.StateMachine
{
    /// <inheritdoc />
    /// <summary>
    /// This class enables easy transitions between multiple windows of the new UI System. 
    /// Uses the Animator component to animate transitions. There must be an "Open" and "Closed"
    /// animation state. For performance reasons it is recommended to use TransitionAlphaBlend
    /// whenever possible.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/State Machine/Transition Animator", 4)]
    [RequireComponent(typeof(Animator))]
    public class TransitionAnimator : StateListener
    {
        [SerializeField] private bool moveToFront = true;

        private Animator animator;

        //Hash of the parameter we use to control the transitions.
        private int m_OpenParameterId;

        //Animator State and Transition names we need to check against.
        const string k_OpenTransitionName = "Open";
        const string k_ClosedStateName = "Closed";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);
        }

        public void Open()
        {
            Activated();
        }

        public void Close()
        {
            Deactivated();
        }

        /// <summary>
        /// Toggle (open / close) this window.
        /// </summary>
        public void OpenClose()
        {
            if (gameObject.activeSelf)
                Deactivated();
            else
                Activated();
        }

        protected override void Activated()
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
            animator.enabled = true;
            if (moveToFront) transform.SetAsLastSibling();
            animator.SetBool(m_OpenParameterId, true);

#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Opened", debug);
#endif
        }

        protected override void Deactivated(bool atStart = false)
        {
            //Start Coroutine to disable the hierarchy when closing animation finishes.
            StopAllCoroutines();

            animator.SetBool(m_OpenParameterId, false);

            StartCoroutine(DisablePanelDelayed());
        }

        /// <summary>
        /// Coroutine that will detect when the Closing animation is finished and it will
        /// deactivate the hierarchy.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DisablePanelDelayed()
        {
            //Start the close animation.
            var closedStateReached = false;
            var shouldBeClosed = true;
            
            while (!closedStateReached && shouldBeClosed)
            {
                if (!animator.IsInTransition(0))
                    closedStateReached = animator.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

                shouldBeClosed = !animator.GetBool(m_OpenParameterId);

                yield return new WaitForEndOfFrame();
            }

            if (!shouldBeClosed) yield break;

#if UNITY_EDITOR
            Logging.Log(this, "'" + gameObject.name + "' Closed", debug);
#endif
            gameObject.SetActive(false);
            animator.enabled = false;
        }
    }
}