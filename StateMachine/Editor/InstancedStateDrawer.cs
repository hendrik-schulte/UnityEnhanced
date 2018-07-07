#if UNITY_EDITOR
using UE.Instancing;
using UnityEditor;

namespace UE.StateMachine
{
    [CustomPropertyDrawer(typeof(InstancedState))]
    public class InstancedStateDrawer : InstanceReferenceDrawer
    {
        protected override string InstanciablePropertyName => "state";

        protected override IInstanciable Instanciable(SerializedProperty property)
        {
            //Overiding this to access the statemanager rather than the state itself
            return (GetInstanciableProperty(property).objectReferenceValue as State)?.stateManager;
        }
    }
}
#endif