#if UE_Photon
using UE.PUNNetworking;
#endif
using UnityEngine;

namespace UE.Instancing
{
    /// <summary>
    /// This is a unified reference object for <see cref="InstanciableSO{T}"/>. It intruduces an abstraction
    /// layer so that instancing is enabled by default. The keys are defined in the inspector in case of an
    /// instanced state machine.
    /// </summary>
    public abstract class InstanceReference : IInstanceReference
    {
#if UNITY_EDITOR
        public static string WARNING_INSTANCE_KEY_EMPTY =
            "Your target asset has instancing enabled but you did not assign an Instance Key.";
#endif

        [SerializeField]
        [Tooltip("This is the key to an instance of the ScriptableObject below. Allows to reference " +
                 "a specific instance of the Object by hashing the key. This may be the root of a " +
                 "prefab to create an instance for every instance of the prefab.")]
        private Object key;

        /// <summary>
        /// Instance Key.
        /// </summary>
        public Object Key
        {
            get { return key; }
            set { key = value; }
        }

        public abstract IInstanciable Target { get; }

#if UNITY_EDITOR && UE_Photon
        public bool HasValidNetworkingKey => PhotonSync.ValidNetworkingKey(Target, Key);
#endif
    }
}