#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace UE.Instancing
{
    /// <summary>
    /// This component allows to change all instance keys of an object hierachy at once.
    /// </summary>
    public class SetInstanceKeyInChildren : MonoBehaviour
    {
        public Object instanceKey;

        public void Apply()
        {
            var instanceObservers = GetComponentsInChildren<InstanceObserver>(true);

            Undo.RecordObjects(instanceObservers, "Batch-Applying Instance Key for InstanceObservers in Hierachy.");

            foreach (var iO in instanceObservers)
            {
                iO.SetKey(instanceKey);
            }
        }
    }

    [CustomEditor(typeof(SetInstanceKeyInChildren), true)]
    public class SetInstanceInChidrenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var setInstanceKeyInChildren = target as SetInstanceKeyInChildren;

            if (GUILayout.Button("Apply"))
            {
                setInstanceKeyInChildren.Apply();
            }
        }
    }
}
#endif