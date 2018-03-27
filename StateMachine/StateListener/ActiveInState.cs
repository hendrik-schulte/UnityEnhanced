namespace UE.StateMachine
{
    /// <summary>
    /// This component activates this game object when one of the given states
    /// is activated and disables it as soon as the state is left.
    /// </summary>
    public class ActiveInState : StateListener
    {
        protected override void Activated()
        {
            gameObject.SetActive(true);
        }

        protected override void Deactivated(bool atStart = false)
        {
            gameObject.SetActive(false);
        }
    }
}