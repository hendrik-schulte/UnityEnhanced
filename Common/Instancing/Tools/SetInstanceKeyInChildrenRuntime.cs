using System.Collections;
using UE.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Instancing
{
    /// <summary>
    /// This component allows to change all instance keys of an object hierachy at once.
    /// </summary>
    public class SetInstanceKeyInChildrenRuntime : MonoBehaviour
    {
        [SerializeField] private GameObjectReference instanceKey;

        [SerializeField] private UnityEvent onApplyRuntime;

        private IEnumerator Start()
        {
            if (!instanceKey.UseConstant && instanceKey.Variable == null)
                yield break;

            //Waiting for the variable to be set.
            while (!instanceKey.Value) yield return null;

            Refresh();
        }

        /// <summary>
        /// Applies the instanceKey object to the entire hierachy.
        /// </summary>
        public void Refresh()
        {
            InstancingTools.ApplyToHierarchy(gameObject, instanceKey.Value);
            onApplyRuntime.Invoke();
        }
    }
}