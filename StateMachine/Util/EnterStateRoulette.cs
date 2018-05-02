using System.Collections;
using UE.Common;
using UE.Instancing;
using UnityEngine;

namespace UE.StateMachine
{
    /// <summary>
    /// Randomly enters the target state.
    /// </summary>
    public class EnterStateRoulette : InstanceObserver
    {
        [SerializeField]
        private State targetState;
        
        [SerializeField] 
        [Range(0,1)]
        [Tooltip("Every second, this is the chance to enter the given state.")]
        private float chance;

        [SerializeField] 
        [Range(0,0.1f)]
        [Tooltip("Every second, this value is acumulated to the chance.")]
        private float increaseOverTime;   

        [SerializeField] 
        private bool debugLog; 
        
        private void OnEnable()
        {
            StartCoroutine(RouletteBreak());
        }

        private IEnumerator RouletteBreak()
        {
            var bonusChance = 0f;
            
            while (true)
            {
                if (!targetState.IsActive(key))
                {
                    var currentChance = chance + bonusChance;
                
                    if (currentChance > Random.value)
                    {
                        Logging.Log(this,"Roulette positive", debugLog);
                        bonusChance = 0;
                        targetState.Enter(key);
                    }

                    bonusChance += increaseOverTime;
                    Logging.Log(this,"Current Chance " + currentChance.ToString("F2"), debugLog && increaseOverTime > 0);
                }
                
                yield return new WaitForSeconds(1);
            }
        }

        public override IInstanciable GetTarget()
        {
            return !targetState ? null : targetState.stateManager;
        }
    }
}