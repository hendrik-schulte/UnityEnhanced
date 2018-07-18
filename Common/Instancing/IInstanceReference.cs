using UnityEngine;

namespace UE.Instancing
{
    public interface IInstanceReference
    {
        /// <summary>
        /// The instance key defined. Use this for all calls on the instanciated object.
        /// </summary>
        Object Key { get; set; }
        
        /// <summary>
        /// Returns the instanciated object. It is used to display the instance key property in the inspector
        /// only when instancing is enabled for the returned object.
        /// </summary>
        /// <returns></returns>
        IInstanciable Target { get; }

        /// <summary>
        /// Returns true, if the reference is null;
        /// </summary>
        bool IsNull { get; }

#if UNITY_EDITOR && UE_Photon
        /// <summary>
        /// Returns true if the assigned key fulfills the requirements for Photon sync.
        /// </summary>
        bool HasValidNetworkingKey { get; }
#endif
    }
}