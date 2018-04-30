using System.Collections;
using UE.Common;
using UE.Instancing;
using UE.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Interaction
{
    public class OnDistance : InstanceObserver
    {
        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;

        public TransformReference target;

        public EnterMode Mode;

        [Header("Restrictions")] public float Threshold;

        [Header("Update Frequency")] [Tooltip("Defines the frequency by which the distance check is performed.")]
        [SerializeField]
        private FloatReference UpdateInterval = new FloatReference(0);

        [Header("Response")] public UnityEvent OnTriggered;

        public enum EnterMode
        {
            IsCloserThan,
            IsFurtherThan
        }

        protected virtual void OnEnable()
        {
            StartCoroutine(CheckDistance());
        }

        private IEnumerator CheckDistance()
        {
            while (!target.Value)
            {
                Logging.Log(this, "Target for proximity check is null!", Logging.Level.Warning, loggingLevel);
                yield return new WaitForSeconds(.2f);
            }

            Logging.Log(this, "Target found.", Logging.Level.Verbose, loggingLevel);

            while (true)
            {                                
                Logging.Log(this, "Performing distance check.", Logging.Level.Verbose, loggingLevel);

                var distance = Vector3.Distance(transform.position, target.Value.position);

                if ((Mode == EnterMode.IsCloserThan && distance < Threshold) ||
                    (Mode == EnterMode.IsFurtherThan && distance > Threshold))
                {
                    Logging.Log(this, "State entered", Logging.Level.Info, loggingLevel);

                    OnTriggered.Invoke();
                    Triggered();
                }
                
                if (UpdateInterval == 0) yield return null;
                else yield return new WaitForSeconds(UpdateInterval);
            }
        }

        protected virtual void Triggered()
        {
        }

        public override IInstanciable GetTarget()
        {
            return null;
        }
    }
}