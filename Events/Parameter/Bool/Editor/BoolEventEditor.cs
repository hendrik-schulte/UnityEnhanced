#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace UE.Events
{
    [CustomEditor(typeof(BoolEvent), true)]
    [CanEditMultipleObjects]
    public class BoolEventEditor : ParameterEventEditor<bool, BoolEvent>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            var boolEvent = target as BoolEvent;

            GUI.enabled = Application.isPlaying;

            if (boolEvent.Instanced)
            {
                if (GUILayout.Button("Raise (true) for all Instances"))
                    boolEvent.RaiseAllInstances(true);
                if (GUILayout.Button("Raise (false) for all Instances"))
                    boolEvent.RaiseAllInstances(false);
            }
            else
            {
                if (GUILayout.Button("Raise (true)"))
                    boolEvent.Raise(true);
                
                if (GUILayout.Button("Raise (false)"))
                    boolEvent.Raise(false);
            }
        }
    }
}
#endif