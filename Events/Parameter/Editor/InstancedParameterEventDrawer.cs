#if UNITY_EDITOR
using UE.Instancing;

namespace UE.Events
{
    public abstract class InstancedParameterEventDrawer<T, TS> : InstanceReferenceDrawer where TS : ParameterEvent<T, TS>
    {
        protected override string InstanciablePropertyName => "paramEvent";
    }
}
#endif