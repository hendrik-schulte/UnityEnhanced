// ----------------------------------------------------------------------------
// Based on Work from Ryan Hipple, Unite 2017 - Game Architecture with Scriptable Objects
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UE.Common;
using UE.Instancing;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UE_Photon
using UE.PUNNetworking;
#endif

namespace UE.Events
{
    [CreateAssetMenu(menuName = "Events/Event()")]
    public class GameEvent : InstanciableSO<GameEvent>
    {
#if UE_Photon
        [SerializeField, HideInInspector] 
        [Tooltip("Enables automatic sync of this event in a Photon network.\n" +
                 "You need to have a PhotonSync Component in your Scene and the " +
                 "event asset needs to be located at the root of a resources folder" +
                 "with a unique name.")]
        private bool PUNSync;
        
        [SerializeField, HideInInspector] private EventCaching CachingOptions;
#endif

        [SerializeField] private bool debugLog;

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif

        [SerializeField] private UnityEvent OnEventTriggered = new UnityEvent();

        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>i
        private readonly List<GameEventListener> eventListeners =
            new List<GameEventListener>();

        /// <summary>
        /// Raises the event. Does not work with instancing.
        /// </summary>
        public void Raise()
        {
            Raise(null);
        }

        public void Raise(Object key)
        {
            Logging.Log(this, name + " was raised!", debugLog);

            for (var i = Instance(key).eventListeners.Count - 1; i >= 0; i--)
                Instance(key).eventListeners[i].OnEventRaised();
            Instance(key).OnEventTriggered.Invoke();

#if UE_Photon

            if (PUNSync && PhotonNetwork.inRoom)
            {
                var raiseEventOptions = new RaiseEventOptions()
                {
                    CachingOption = CachingOptions,
                    Receivers = ReceiverGroup.Others
                };

                PhotonNetwork.RaiseEvent(PhotonSync.EventStateChange, name, true, raiseEventOptions);
            }
#endif
        }

        public void RegisterListener(GameEventListener listener, Object key = null)
        {
            if (!Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Add(listener);
        }

        public void RegisterListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.AddListener(listener);
        }

        public void UnregisterListener(GameEventListener listener, Object key = null)
        {
            if (Instance(key).eventListeners.Contains(listener))
                Instance(key).eventListeners.Remove(listener);
        }

        public void UnregisterListener(UnityAction listener, Object key = null)
        {
            Instance(key).OnEventTriggered.RemoveListener(listener);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(GameEvent), true)]
    [CanEditMultipleObjects]
    public class EventEditor : InstanciableSOEditor
    {
#if UE_Photon
        private SerializedProperty PUNSync;
        private SerializedProperty CachingOptions;
#endif
        
        protected override void OnEnable()
        {
            base.OnEnable();
            

#if UE_Photon
            PUNSync = serializedObject.FindProperty("PUNSync");
            CachingOptions = serializedObject.FindProperty("CachingOptions");
#endif
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameEvent = target as GameEvent;

            GUI.enabled = Application.isPlaying && !gameEvent.Instanced;

            if (GUILayout.Button("Raise"))
                gameEvent.Raise();
        }


        protected override void OnInspectorGUITop()
        {
#if UE_Photon
            serializedObject.Update();
            ScriptableObjectEditorUtility.PhotonControl(PUNSync, CachingOptions);
            serializedObject.ApplyModifiedProperties();
#endif
        }
    }
#endif
}