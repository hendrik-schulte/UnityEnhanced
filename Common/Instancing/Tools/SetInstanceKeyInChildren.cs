using UE.Instancing;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace VRMP.Scripts.Util
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

#if UNITY_EDITOR
            Debug.Log("Editor Undo stuff on " + gameObject.name);
            Undo.RecordObjects(instanceObservers, "Batch-applying instance key to object hierachy.");
#endif

            foreach (var iO in instanceObservers)
            {
                iO.SetKey(instanceKey);
            }
        }
    }

#if UNITY_EDITOR
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
#endif
}