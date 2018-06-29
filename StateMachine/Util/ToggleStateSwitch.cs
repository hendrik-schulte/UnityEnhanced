using UE.Instancing;
using UnityEngine;
using UnityEngine.UI;

namespace UE.StateMachine
{
    /// <summary>
    /// Connects to the local toggle component and enters the given states when the toggle is used.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class ToggleStateSwitch : InstanceObserver
    {
        public State enabledState;
        public State disabledState;
        
        private void Awake()
        {
            var toggle = GetComponent<Toggle>();

            enabledState.stateManager.Init(Key);
            
            toggle.isOn = enabledState.IsActive(Key);
            
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }

        private void OnDestroy()
        {
            var toggle = GetComponent<Toggle>();
            
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }

        private void OnToggleChanged(bool value)
        {
            if(value) enabledState.Enter(Key);
            else disabledState.Enter(Key);
        }

        public override IInstanciable Target => enabledState?.stateManager;
    }
}