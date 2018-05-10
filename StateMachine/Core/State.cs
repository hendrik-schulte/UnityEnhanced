using System.Linq;
using UE.Common.SubjectNerd.Utilities;
using UE.Instancing;
using UE.UI;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// A state asset used within the state machine.
    /// </summary>
    [CreateAssetMenu(menuName = "State Machine/State")]
    public class State : ScriptableObject
    {
        [Tooltip("This is the manager in wich this state is managed.")]
        [EditScriptable]
        public StateManager stateManager;

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        /// <summary>
        /// This state is activated.
        /// </summary>
        public void Enter()
        {
            stateManager.SetState(this);
        }

        /// <summary>
        /// This state is activated using an instance key.
        /// </summary>
        /// <param name="key">key to an instance of state manager</param>
        public void Enter(Object key)
        {
            stateManager.SetState(this, key);
        }
        
        
        /// <summary>
        /// Enters this state for all instances of this state machine.
        /// </summary>
        public void EnterAllInstances()
        {
            stateManager.SetStateAllInstances(this);
        }

        /// <summary>
        /// Returns true if this state is currently activated.
        /// </summary>
        /// <returns></returns>
        public bool IsActive()
        {
            return IsActive(null);
        }

        /// <summary>
        /// Returns true if this state is currently activated in the state managers instance.
        /// </summary>
        /// <param name="key">key to an instance of state manager</param>
        /// <returns></returns>
        public bool IsActive(Object key)
        {
            if (!stateManager) return false;

            return stateManager.GetState(key) == this;
        }

        /// <summary>
        /// Returns true if all state machine instances are currently in this state.
        /// </summary>
        /// <returns></returns>
        public bool AllInstancesActive(bool includeMain = false)
        {
            if (!stateManager) return false;

            if (!stateManager.Instanced) return IsActive();

            var instance = stateManager.GetInstances();

            if (includeMain) if (!IsActive()) return false;

            return instance.All(v => v.GetState() == this);
        }

        /// <summary>
        /// This returns true, when the state is the initial state of this system.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialState()
        {
            if (!stateManager) return false;

            return stateManager.InitialState == this;
        }

        /// <summary>
        /// This draws a state gizmo at the given world space position.
        /// </summary>
        /// <param name="position"></param>
        public void DrawWorldSpaceGizmo(Vector3 position, Color? color = null)
        {
            Gizmo.DrawWorldSpaceString("Current: " + name, position, color);
        }
    }
}