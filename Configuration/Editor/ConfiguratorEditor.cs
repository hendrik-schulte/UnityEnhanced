#if UNITY_EDITOR && UE_SharpConfig
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;

namespace UE.Configuration.Editor
{
    [CustomEditor(typeof(Configurator))]
    [CanEditMultipleObjects]
    public class ConfiguratorEditor : ReorderableArrayInspector
    {
        protected override void InitInspector()
        {
            base.InitInspector();
        }

        protected override void DrawInspector()
        {
            base.DrawInspector();
        }
    }
}
#endif