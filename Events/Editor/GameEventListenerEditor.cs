#if UNITY_EDITOR
using System.Collections.Generic;
using UE.Instancing;
using UnityEditor;

namespace UE.Events
{
    [CustomEditor(typeof(GameEventListener), true)]
    [CanEditMultipleObjects]
    public class GameEventListenerEditor : InstanceObserverEditor
    {
        protected override IEnumerable<string> ExcludeProperties()
        {
            if ((target as GameEventListener).DrawUnityEventInspector)
                return base.ExcludeProperties();
            else
                return new[] {"Response"};
        }
    }
}
#endif