using UE.Common;
#if UE_Photon
using UE.PUNNetworking;
#endif
using UE.StateMachine;
using UnityEngine;

namespace UE.Instancing
{
    /// <inheritdoc />
    /// <summary>
    /// This class can be inherited to utilize the ScriptableObject instancing feature.
    /// By defining a key object, you access a specific instance of the referenced
    /// InstanciableSO. The key is used within the entire class. Is is recommended to
    /// use the respective InstancedX wrappers (e.g. <see cref="InstancedState"/>) to
    /// access objects instead of this class, to be more flexible.
    /// </summary>
    public abstract class InstanceObserver : MonoBehaviour, IInstanceReference
    {
        [SerializeField, HideInInspector] private Object _key;

        public virtual Object Key
        {
            get { return _key; }
            set
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Logging.Warning(this, "Setting instance key at runtime. This is not recommended " +
                                          "and might cause undefined behaviour.");
#endif
                _key = value;
            }
        }

        /// <summary>
        /// Returns the instanciated object. It is used to display the instance key property in the inspector
        /// only when instancing is enabled for the returned object.
        /// </summary>
        /// <returns></returns>
        public abstract IInstanciable Target { get; }

        public bool IsNull => Target == null;

#if UNITY_EDITOR && UE_Photon
        public bool HasValidNetworkingKey => PhotonSync.ValidNetworkingKey(Target, Key);
#endif
    }
}