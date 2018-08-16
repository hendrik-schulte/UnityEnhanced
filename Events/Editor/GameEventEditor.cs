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
        protected override void DrawInspector()
        {
            base.DrawInspector();

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

        protected override string[] ExcludeProperties()
        {
            if ((target as GameEvent).DrawUnityEventInspector)
                return base.ExcludeProperties();
            else
                return new[] {"OnEventTriggered"};
        }
    }
}
#endif