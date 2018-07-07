#if UNITY_EDITOR
using UE.Instancing;
using UnityEditor;

namespace UE.Events
{
    [CustomPropertyDrawer(typeof(InstancedGameEvent))]
    public class InstancedGameEventDrawer : InstanceReferenceDrawer
    {
        protected override string InstanciablePropertyName => "gameEvent";
    }
}
#endif