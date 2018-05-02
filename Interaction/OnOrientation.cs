using System.Collections;
using UE.Common;
using UE.Instancing;
using UE.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace UE.Interaction
{
    public class OnOrientation : InstanceObserver
    {        
        public TransformReference target;

        [Tooltip("Right - Red - X\nUp - Green - Y\nForward - Blue - Z")]
        public EnterMode Mode = EnterMode.Forward;

        [Tooltip("Defines the frequency by which the orientation check is performed.")]
        [SerializeField]
        private FloatReference UpdateInterval = new FloatReference(0);
        
        [SerializeField] protected Logging.Level loggingLevel = Logging.Level.Warning;
        
        [Tooltip("The angle between the forward vectors of the targets needs to be smaller than this.")]
        [Range(0, 90)]
        public float ThresholdAngle;

        public enum EnterMode
        {
            Forward,
            Right,
            Up
        }

        [Header("Response")] public UnityEvent OnTriggered;
        
        protected virtual void OnEnable()
        {
            StartCoroutine(CheckDistance());
        }

        private IEnumerator CheckDistance()
        {
            yield return null;
            
            while (!target.Value)
            {
                Logging.Log(this, "Target for orientation check is null!", Logging.Level.Warning, loggingLevel);
                yield return new WaitForSeconds(.2f);
            }

            Logging.Log(this, "Target found.", Logging.Level.Verbose, loggingLevel);

            while (true)
            {
                float angle;

                switch (Mode)
                {
                    case EnterMode.Forward:
                        angle = Vector3.Angle(transform.forward, target.Value.forward);
                        break;
                    case EnterMode.Right:
                        angle = Vector3.Angle(transform.right, target.Value.right);
                        break;
                    case EnterMode.Up:
                        angle = Vector3.Angle(transform.up, target.Value.up);
                        break;
                    default:
                        angle = -1;
                        break;
                }

                Logging.Log(this, "Performing orientation check. Angle: " + angle, Logging.Level.Verbose, loggingLevel);

                if (angle < ThresholdAngle)
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