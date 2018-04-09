using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    public class StateMachineDebug : InstanceObserver
    {
        [SerializeField] private StateManager stateManager;

        [SerializeField] private bool onlyWhenSelected;

        public override IInstanciable GetTarget()
        {
            return stateManager;
        }
        
        private void Start(){}

        private void OnDrawGizmos()
        {
            if (!stateManager || !enabled) return;

            if (!onlyWhenSelected) Draw();
        }

        private void OnDrawGizmosSelected()
        {
            if (!stateManager|| !enabled) return;

            if (onlyWhenSelected) Draw();
        }

        private void Draw()
        {
            stateManager.DrawWorldSpaceGizmo(transform.position, key);
        }
    }
}