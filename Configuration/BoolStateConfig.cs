#if UE_SharpConfig
using System;
using UE.StateMachine;
using UnityEngine;

namespace UE.Configuration
{
    [Serializable]
    public class BoolStateConfig : ConfigEntry
    {
        [SerializeField]
        private State trueState;
        [SerializeField]
        private State falseState;

        public override void Setup(SharpConfig.Setting setting)
        {
                falseState.stateManager.Init();
                setting.BoolValue = trueState.IsActive();
                setting.Comment = comment;
        }

        public override void Load(SharpConfig.Setting setting)
        {
            if (setting.BoolValue)
                trueState.Enter();
            else
                falseState.Enter();
        }
    }
}
#endif