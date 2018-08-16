#if UE_SharpConfig
using System;
using UE.Variables;

namespace UE.Configuration
{
    [Serializable]
    public class FloatConfig : ConfigEntry
    {
        public FloatVariable value;
        
        public override void Setup(SharpConfig.Setting setting)
        {
            setting.FloatValue = value.Value;
            setting.Comment = comment;
        }

        public override void Load(SharpConfig.Setting setting)
        {
            value.Value = setting.FloatValue;
        }
    }
}
#endif