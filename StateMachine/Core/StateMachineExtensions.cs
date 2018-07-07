using System.Collections.Generic;
using System.Linq;

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
    }
}