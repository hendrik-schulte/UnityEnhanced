using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    public class StateMachineDebug : InstanceObserver
    {
        [SerializeField] private StateManager stateManager;

        [SerializeField] private bool onlyWhenSelected;
//        [SerializeField] private bool onlyInPlayMode;

        public override IInstanciable GetTarget()
        {
            return stateManager;
        }
        
        private void Start(){}

        private void OnDrawGizmos()
        {
            if (!onlyWhenSelected) Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (onlyWhenSelected) Draw();
        }

        private void Draw()
        {
            if (!stateManager || !enabled || !Application.isPlaying) return;

            stateManager.DrawWorldSpaceGizmo(transform.position, key);
        }
    }
}