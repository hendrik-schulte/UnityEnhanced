using UnityEngine;

namespace UE.Instancing
{
    /// <summary>
    /// This component allows to change all instance keys of an object hierarchy at once.
    /// </summary>
    public class SetInstanceKeyInChildren : MonoBehaviour
    {
        public Object instanceKey;

        [ContextMenu("Apply")]
        public void Apply()
        {
            InstancingTools.ApplyToHierarchy(gameObject, instanceKey);
        }
    }
}