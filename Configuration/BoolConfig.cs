#if UE_SharpConfig
using System;
using UE.Variables;

namespace UE.Configuration
{
    [Serializable]
    public class BoolConfig : ConfigEntry
    {
        public BoolVariable value;

        public override void Setup(SharpConfig.Setting setting)
        {
            setting.BoolValue = value.Value;
            setting.Comment = comment;
        }

        public override void Load(SharpConfig.Setting setting)
        {
            value.Value = setting.BoolValue;
        }
    }
}
#endif