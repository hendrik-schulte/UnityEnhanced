#if UNITY_EDITOR
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
#endif
using UnityEngine;

namespace UE.Instancing
{
    /// <summary>
    /// This component allows to change all instance keys of an object hierachy at once.
    /// </summary>
    public class SetInstanceKeyInChildren : MonoBehaviour
    {
        public Object instanceKey;

        [ContextMenu("Apply")]
        public void Apply()
        {
            InstancingTools.ApplyToHierachy(gameObject, instanceKey);
        }
    }
}