using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// Displays a gizmo at the position of this object that shows the current state.
    /// </summary>
    public class StateMachineDebug : InstanceObserver
    {
        [SerializeField] private StateManager stateManager;

        [SerializeField] private bool onlyWhenSelected;
        
        [SerializeField] private Color color = Color.white;

        public override IInstanciable GetTarget()
        {
            return stateManager;
        }

        private void Start()
        {
            //To have an enabled checkbox
        }

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

            stateManager.DrawWorldSpaceGizmo(transform.position, color, key);
        }
    }
}