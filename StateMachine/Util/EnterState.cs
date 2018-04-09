using UnityEngine;

namespace UE.StateMachine
{
    public class EnterState : MonoBehaviour
    {
        public State state;
        public State back;

        public void Enter(bool enter)
        {
            if(enter) state.Enter();
            else back.Enter();
        }
    }
}