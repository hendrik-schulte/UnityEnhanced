#if UE_SharpConfig
using System;
using UE.Variables;

namespace UE.Configuration
{
    [Serializable]
    public class IntConfig : ConfigEntry
    {
        public IntVariable value;

        public override void Setup(SharpConfig.Setting setting)
        {
            setting.IntValue = value.Value;
            setting.Comment = comment;
        }

        public override void Load(SharpConfig.Setting setting)
        {
            value.Value = setting.IntValue;
        }
    }
}
#endif