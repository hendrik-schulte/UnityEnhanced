using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StateMachine;
using UnityEngine;
using UnityEngine.Events;

namespace StateMachine
{
    /// <summary>
    /// This class enables easy transitions between multiple windows of the new UI System. 
    /// Uses the Animator component to animate transitions. There must be an "Open" and "Closed"
    /// animation state. Can be state-based (only active in selected game states) or event based 
    /// (needs to be opened or closed directly via reference, for example by a Button).
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class Transition : MonoBehaviour
    {
        [SerializeField] private bool debugLog;
        [SerializeField] private bool enabledOnStart;

        [SerializeField] private bool moveToFront = true;
//        [SerializeField] private bool animated = true;

        [Header("State Based")] [SerializeField]
        private List<State> activeStates;

        private Animator anim;

        //Hash of the parameter we use to control the transitions.
        private int m_OpenParameterId;

        //Animator State and Transition names we need to check against.
        const string k_OpenTransitionName = "Open";
        const string k_ClosedStateName = "Closed";

        public UnityEvent OnActivated;
        public UnityEvent OnDeactivated;

        void Awake()
        {
            anim = GetComponent<Animator>();
            m_OpenParameterId = Animator.StringToHash(k_OpenTransitionName);

            if (activeStates.Any())
            {
                activeStates[0].stateManager?.OnStateEnter.AddListener(OnStateEnter);
                if (enabledOnStart) activeStates[0].stateManager.State = activeStates[0];
            }

            if (enabledOnStart)
            {
                Open(false);
            }
            else
            {
                Close(false);
            }
        }

        /// <summary>
        /// Activates or deactivates the window based on the state that is beeing activated.
        /// </summary>
        /// <param name="state"></param>
        private void OnStateEnter(State state)
        {
            var previouslyActive = gameObject.activeSelf;
            var active = activeStates.Any(s => s == state);

            if (previouslyActive && !active)
            {
                OnDeactivated.Invoke();
                Close();
            }

            if (!previouslyActive && active)
            {
                Open();
                OnActivated.Invoke();
            }
        }

        /// <summary>
        /// Toggle (open / close) this window.
        /// </summary>
        /// <param name="animOverride"></param>
        public void OpenClose(bool animOverride = true)
        {
            if (gameObject.activeSelf)
            {
                OnDeactivated.Invoke();
                Close(animOverride);
            }
            else
            {
                Open(animOverride);
                OnActivated.Invoke();
            }
        }

        /// <summary>
        /// Closes the currently open panel and opens the provided one.
        /// It also takes care of handling the navigation, setting the new Selected element.
        /// </summary>
        /// <param name="animOverride">Set to false to ignore animation</param>
        public void Open(bool animOverride = true)
        {
            gameObject.SetActive(true);
            if (moveToFront) transform.SetAsLastSibling();
            anim.SetBool(m_OpenParameterId, true);

            if (!animOverride)
            {
                if (debugLog) print("enabled without animation");
                anim.Play("Open", -1, 1);
                return;
            }
        }

        /// <summary>
        /// Closes the currently open Screen. It also takes care of navigation.
        /// Reverting selection to the Selectable used before opening the current screen.
        /// </summary>
        /// <param name="animOverride">Set to false to ignore animation</param>
        public void Close(bool animOverride = true)
        {
            if (!animOverride)
            {
                gameObject.SetActive(false);
                if (debugLog) print("disabled without animation");
                return;
            }

            //Start the close animation.
            anim.SetBool(m_OpenParameterId, false);

            //Start Coroutine to disable the hierarchy when closing animation finishes.
            StartCoroutine(DisablePanelDelayed());
        }

        /// <summary>
        /// Coroutine that will detect when the Closing animation is finished and it will
        /// deactivate the hierarchy.
        /// </summary>
        /// <returns></returns>
        IEnumerator DisablePanelDelayed()
        {
            bool closedStateReached = false;
            bool shouldBeClosed = true;
            while (!closedStateReached && shouldBeClosed)
            {
                if (!anim.IsInTransition(0))
                    closedStateReached = anim.GetCurrentAnimatorStateInfo(0).IsName(k_ClosedStateName);

                shouldBeClosed = !anim.GetBool(m_OpenParameterId);

                yield return new WaitForEndOfFrame();
            }

            if (!shouldBeClosed) yield break;

            if (debugLog) print("disabled with animation");

            anim.gameObject.SetActive(false);
        }
    }
}