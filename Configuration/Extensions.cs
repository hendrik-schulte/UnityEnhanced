#if UE_SharpConfig
using SharpConfig;
using UE.Common;

namespace UE.Configuration
{
    public static class Extensions
    {
        /// <summary>
        /// Checks if the given section/setting combination exists in this configuration.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="loggingLevel"></param>
        /// <returns></returns>
        public static bool Exists(this SharpConfig.Configuration config, string section, string key, Logging.Level loggingLevel = Logging.Level.Error)
        {
            var exists = config.Contains(section) && config[section].Contains(key);

            if (!exists)
                Logging.Log(config, "Could not find [" + section + "] [" + key + "]", Logging.Level.Warning, loggingLevel);

            return exists;
        }
        
        public static Setting Get(this SharpConfig.Configuration config, ConfigEntry entry)
        {
            return config[entry.section][entry.setting];
        }
    }
}
#endif