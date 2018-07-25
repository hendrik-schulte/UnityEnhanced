using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UE.StateMachine
{
    public static class StateMachineExtensions
    {
        /// <summary>
        /// Returns true if all given states share the same StateManager.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static bool StatesShareStateManager(this List<State> states)
        {
            if (!states.Any()) return true;

            foreach (var state in states)
            {
                if (state == null || state.stateManager == states[0].stateManager) continue;

                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Returns true if at least one state of the given collection is currently active.
        /// </summary>
        /// <param name="states"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsActiveState(this IEnumerable<State> states, Object key)
        {
            return states.Any(state => state.IsActive(key));
        }
        
        /// <summary>
        /// Returns true if at least one state of the given collection is currently active.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        public static bool ContainsActiveState(this IEnumerable<State> states)
        {
            return states.Any(state => state.IsActive());
        }
    }
}