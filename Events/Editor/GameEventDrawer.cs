#if UNITY_EDITOR
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
using UE.Instancing;
using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    /// <summary>
    /// This adds a property drawer for GameEvent fields that allows you to fire that event directly.
    /// </summary>
    [CustomPropertyDrawer(typeof(GameEvent))]
    public class GameEventDrawer : ButtonPropertyDrawer<GameEvent>
    {
        protected override bool EnableButton(GameEvent gameEvent)
        {
            return gameEvent && Application.isPlaying;
        }

        protected override void DrawButton(Rect buttonRect, SerializedProperty property, GameEvent gameEvent)
        {
            if (!GUI.Button(buttonRect, "Raise", buttonStyle)) return;
            
            var parent = property.GetParent<object>();
            var observer = parent as IInstanceReference;

            if (observer == null)
                if (gameEvent.Instanced)
                    gameEvent.RaiseAllInstances();
                else
                    gameEvent.Raise();
            else
                gameEvent.Raise(observer.Key);
        }
    }
}
#endif