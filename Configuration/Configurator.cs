#if UE_SharpConfig
using System.Collections.Generic;
using System.IO;
using UE.Common;
using UnityEngine;

namespace UE.Configuration
{
    public class Configurator : MonoBehaviour
    {
        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        [SerializeField] private string filename = "config.cfg";

//        [Reorderable]
        [SerializeField] private List<BoolStateConfig> boolStateConfiguration;
        [SerializeField] private List<BoolConfig> boolConfiguration;
        [SerializeField] private List<FloatConfig> floatConfiguration;
        [SerializeField] private List<IntConfig> intConfiguration;


        private void Awake()
        {
            if (!enabled) return;

            if (!File.Exists(filename))
            {
                Logging.Log(this, "Setting up a default config since '" + filename + "' was not found!",
                    Logging.Level.Info, loggingLevel);
                SetupConfig(new SharpConfig.Configuration());
                return;
            }

            Logging.Log(this, "Loading config file ...", Logging.Level.Verbose, loggingLevel);
            LoadConfig();
        }

        private void Start()
        {
            //To have an enabled checkbox
        }

        /// <summary>
        /// Creates new config and sets it to default values.
        /// </summary>
        private void SetupConfig(SharpConfig.Configuration config)
        {
            SetupEntries(config, boolStateConfiguration);
            SetupEntries(config, boolConfiguration);
            SetupEntries(config, floatConfiguration);
            SetupEntries(config, intConfiguration);

            config.SaveToFile(filename);
        }

        private void LoadConfig()
        {
            var config = SharpConfig.Configuration.LoadFromFile(filename);
            var error = false;

            LoadEntries(config, boolStateConfiguration, ref error);
            LoadEntries(config, boolConfiguration, ref error);
            LoadEntries(config, floatConfiguration, ref error);
            LoadEntries(config, intConfiguration, ref error);

            if (!error)
            {
                Logging.Log(this, "Loading successful.", Logging.Level.Verbose, loggingLevel);
                return;
            }

            Logging.Log(this, "Couldn't find all entries in the config file. Setting relevant values to " +
                              "default and saving file.", Logging.Level.Warning, loggingLevel);

            SetupConfig(config);
        }

        private static void SetupEntries(SharpConfig.Configuration config, IEnumerable<ConfigEntry> entries)
        {
            foreach (var cfg in entries)
            {
                if (!cfg.Exists()) continue;

                cfg.Setup(config.Get(cfg));
            }
        }

        private void LoadEntries(SharpConfig.Configuration config, IEnumerable<ConfigEntry> entries,
            ref bool error)
        {
            foreach (var cfg in entries)
            {
                if (!cfg.Exists()) continue;

                if (config.Exists(cfg.section, cfg.setting, loggingLevel))
                    cfg.Load(config.Get(cfg));
                else error = true;
            }
        }
    }
}
#endif