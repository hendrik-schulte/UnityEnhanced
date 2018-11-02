using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UE.Common;
using UnityEngine;
using UnityEngine.Events;
#if UE_Photon
using UE.PUNNetworking;
#if PUN_2_OR_NEWER
using Photon.Pun;
#endif
#endif

namespace UE.Instancing
{
    /// <summary>
    /// This class enables Instancing for ScriptableObjects. This needs to be inherited.
    /// After that, all instanced properties must be accessed via Instance(key). The key
    /// is used for a lookup in a dictionary. It is defined in an InstanceObserver to keep
    /// track of the different instaces of this SO. 
    /// </summary>
    /// <typeparam name="T">Derived type</typeparam>
    public abstract class InstanciableSO<T> : ScriptableObject, IInstanciable where T : InstanciableSO<T>
    {        
        [SerializeField, HideInInspector] private bool instanced;

        /// <summary>
        /// In this dictionary the instances of the SO are stored.
        /// </summary>
        private Dictionary<Object, T> instances;

        /// <summary>
        /// A dictionary containing all keys to the instances dictionary. Uses keyID as key.
        /// </summary>
        private Dictionary<int, Object> keys;

        private int keyID = -1;

        /// <summary>
        /// This is to access this instance in the instances dictionary (of the main instance). 
        /// </summary>
        protected int KeyID => keyID;

        /// <summary>
        /// Returns true if instancing is enabled.
        /// </summary>
        public virtual bool Instanced => instanced;

        /// <summary>
        /// Returns the amount of instances registered to this object or 0 is instancing .
        /// </summary>
        public int InstanceCount => instances?.Count ?? 0;

        public string Name => name;
        
#if UE_Photon
        /// <summary>
        /// This is a reference of the original instance. Is null for the original.
        /// </summary>
        protected T original;

        /// <summary>
        /// Returns the Photon Sync settings of this object.
        /// </summary>
        public abstract PhotonSync PhotonSyncSettings { get; }
#endif

        /// <summary>
        /// This event type has the derived type as parameter.
        /// </summary>
        public class InstanciableEvent : UnityEvent<T>
        {
        }

        /// <summary>
        /// This event is triggered whenever instances are added or removed.
        /// </summary>
        public readonly InstanciableEvent OnInstancesChanged = new InstanciableEvent();

        /// <summary>
        /// Removes references to all instances of this ScriptableObject.
        /// </summary>
        protected void Clear()
        {
            instances = instanced ? new Dictionary<Object, T> { } : null;
            OnInstancesChanged.Invoke(this as T);
        }

        /// <summary>
        /// Returns a readonly Collection of all instances of this Object.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<T> GetInstances()
        {
            if (instances == null)
            {
                return new ReadOnlyCollection<T>(new List<T>());
            }

            return new ReadOnlyCollection<T>(instances.Values.ToArray());
        }

        /// <summary>
        /// Returns a collection of all dictionary keys.
        /// </summary>
        public Object[] Keys => instances?.Keys.ToArray();

        /// <summary>
        /// Returns an instance of this ScriptableObject based on the given key object.
        /// Returns the main object if it is null or instancing is not enabled. Should be
        /// used to access all instanced properties of this ScriptableObject.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T Instance(Object key)
        {
            if (!instanced) return (T) this;
            if (key == null)
            {
                Logging.Warning(this, "Accessing the main object of an instanced system. This is " +
                                      "probably not intended. Did you forget to assign an instance key?");
                return (T) this;
            }

            if (instances == null)
            {
                instances = new Dictionary<Object, T>();
                keys = new Dictionary<int, Object>();
            }

            if (!instances.ContainsKey(key))
            {
                var instance = CreateInstance<T>();
                instance.name += "_" + key.GetInstanceID();
#if UE_Photon
                instance.original = this as T;
#endif
                instances.Add(key, instance);
                AddKey(key, instance);
                OnInstancesChanged.Invoke(this as T);
            }

            return instances[key];
        }

        /// <summary>
        /// Returns the object key from the keyID of the instanced object.
        /// For PhotonView, it uses the viewID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Object GetByKeyId(int id)
        {
            return keys[id];
        }

        /// <summary>
        /// Adds the given key Object to a separate dictionary to access it
        /// via its InstanceID. Uses the viewID for PhotonViews.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        private void AddKey(Object key, T instance)
        {
            instance.keyID = key.GetInstanceID();

#if UE_Photon
//When using photon sync, use a photon viewID as key to guarantee matching network instances.
            if (PhotonSyncSettings.PUNSync)
            {
                var photonView = KeyToPhotonView(key);
                if (photonView)
                {
#if PUN_2_OR_NEWER
                    instance.keyID = photonView.ViewID;
#else
                    instance.keyID = photonView.viewID;
#endif
                }
            }
#endif

            if (keys.ContainsKey(instance.keyID))
            {
                //If the taret key object is null (which might be the case by a scene reload),
                //we remove that entry from the dictionary
                if (keys[instance.keyID] == null) keys.Remove(instance.keyID);
                else
                {
                    Logging.Error(this,
                        "Failed adding key '" + instance.keyID + "' -> '" + key.name + "' because it already exists! " +
                        "Existing: '" + instance.keyID + "' -> '" + keys[instance.keyID] + "'");
                    return;
                }
            }

            keys.Add(instance.keyID, key);
        }

#if UE_Photon
        /// <summary>
        /// Tries to parse the photon view directly or find it in the components of the key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private PhotonView KeyToPhotonView(Object key)
        {
            if (key is PhotonView)
                return key as PhotonView;

            if (key is GameObject)
            {
                var view = (key as GameObject).GetComponent<PhotonView>();
                if (view) return view;
            }

            Logging.Error(this, "'" + name + "': Syncable intanced objects need to have a PhotonView " +
                                "or a parenting GameObject as key object!");
            return null;
        }
#endif
    }

    /// <summary>
    /// This interface is implemented by InstanciatedSO and simplifies casting when generic parameter is unknown.
    /// </summary>
    public interface IInstanciable
    {
        /// <summary>
        /// Is instancing enabled for this object?
        /// </summary>
        bool Instanced { get; }

        /// <summary>
        /// How many instances are there?
        /// </summary>
        int InstanceCount { get; }

        /// <summary>
        /// Returns a collection of instance keys.
        /// </summary>
        Object[] Keys { get; }
        
        /// <summary>
        /// Returns the name of the ScriptableObject.
        /// </summary>
        string Name { get; }

#if UE_Photon
        /// <summary>
        /// Returns the Photon Sync settings or null if this object is not syncable.
        /// </summary>
        PhotonSync PhotonSyncSettings { get; }
#endif
    }
}