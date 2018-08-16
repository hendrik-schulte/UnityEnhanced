#if UE_SharpConfig
using System;
using System.Linq;
using UnityEngine;

namespace UE.Configuration
{
    [Serializable]
    public abstract class ConfigEntry
    {
        [Tooltip("Config file section.")] 
        public string section;
        [Tooltip("Config file key.")] 
        public string setting;
        [SerializeField]
        protected string comment;

        /// <summary>
        /// Returns true if this configuration has a valid section and setting defined.
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return section.Any() && setting.Any();
        }

        public abstract void Setup(SharpConfig.Setting setting);
        public abstract void Load(SharpConfig.Setting setting);
    }
}
#endif