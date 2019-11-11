#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UE.Instancing
{
    public static class InstancingTools
    {
        /// <summary>
        /// Applies the given instance key to all InstanceObservers underneath the given GameObject in the hierarchy.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="instanceKey"></param>
        public static void ApplyToHierarchy(GameObject gameObject, Object instanceKey)
        {
            var instanceObservers = gameObject.GetComponentsInChildren<InstanceObserver>(true);

#if UNITY_EDITOR
            if (!Application.isPlaying)
                Undo.RecordObjects(instanceObservers, "Batch-Applying Instance Key for InstanceObservers in hierarchy.");
#endif

            foreach (var iO in instanceObservers)
            {
                iO.Key = instanceKey;
            }
        }
    }
}