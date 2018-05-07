using System.Collections;
using UE.Common;
using UE.Instancing;
using UE.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Interaction
{
    /// <summary>
    /// This component checks the distance between this object and a target object and fires an event as soon as
    /// the object comes closer than the given treshhold distance.
    /// </summary>
    [AddComponentMenu("Unity Enhanced/Interaction/OnDistance")]
    public class OnDistance : InstanceObserver
    {
        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        public TransformReference target;

        public EnterMode Mode;
        [Tooltip("Defines the frequency by which the distance check is performed.")]
        [SerializeField]
        private FloatReference UpdateInterval = new FloatReference(0);
        
        [Header("Restrictions")] 
        public FloatReference Threshold = new FloatReference(1);
        
        [Tooltip("Disable this trigger for given seconds after it was triggered.")] [SerializeField] [Range(0, 5)]
        private float cooldown;

        private bool coolingDown;
        
        [Header("Response")] public UnityEvent OnTriggered;

        public enum EnterMode
        {
            IsCloserThan,
            IsFurtherThan
        }

        protected virtual void OnEnable()
        {
            coolingDown = false;
            
            StartCoroutine(CheckDistance());
        }

        private void CancelCooldown()
        {
            coolingDown = false;
        }
        
        private IEnumerator CheckDistance()
        {
            yield return null;
            
            while (!target.Value)
            {
                Logging.Log(this, "Target for proximity check is null!", Logging.Level.Warning, loggingLevel);
                yield return new WaitForSeconds(.2f);
            }

            Logging.Log(this, "Target found.", Logging.Level.Verbose, loggingLevel);

            while (true)
            {                        
                while (coolingDown)
                    yield return null;    
                
                Logging.Log(this, "Performing distance check.", Logging.Level.Verbose, loggingLevel);

                var distance = Vector3.Distance(transform.position, target.Value.position);

                if ((Mode == EnterMode.IsCloserThan && distance < Threshold) ||
                    (Mode == EnterMode.IsFurtherThan && distance > Threshold))
                {
                    Logging.Log(this, "Distance reached!", Logging.Level.Info, loggingLevel);

                    Triggered();
                }
                
                if (UpdateInterval == 0) yield return null;
                else yield return new WaitForSeconds(UpdateInterval);
            }
        }

        protected virtual void Triggered()
        {            
            coolingDown = true;
            Invoke(nameof(CancelCooldown), cooldown);
            
            OnTriggered.Invoke();
        }

        public override IInstanciable GetTarget()
        {
            return null;
        }
    }
}