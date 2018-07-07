#if UNITY_EDITOR
using UE.Instancing;

namespace UE.Events
{
    public class ParameterEventEditor<T, TS> : InstanciableSOEditor where TS : ParameterEvent<T, TS>
    {
#if UE_Photon
        protected override string[] ExcludeProperties()
        {
            //Making sure the pun sync option is not displayed when the parameter is not syncable.
            var paramEvent = target as ParameterEvent<T, TS>;

            return paramEvent.IsNetworkingType ? base.ExcludeProperties() : new string[] {"PhotonSync"};
        }
#endif
    }
}
#endif