#if UNITY_EDITOR
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(GameEvent), true)]
    [CanEditMultipleObjects]
    public class GameEventEditor : InstanciableSOEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var gameEvent = target as GameEvent;

            GUI.enabled = Application.isPlaying;

            if (gameEvent.Instanced)
            {
                if (GUILayout.Button("Raise for all Instances"))
                    gameEvent.RaiseAllInstances();
            }
            else
            {
                if (GUILayout.Button("Raise"))
                    gameEvent.Raise();
            }
        }
    }
}
#endif