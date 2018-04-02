using System.Collections;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UE.UI
{
    /// <summary>
    /// This component is used to smooth a progress bar.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ProgressBar : MonoBehaviour
    {
//    [SerializeField, Range(0, 1f)]
//    private float InspectorVal;
//
//    private void OnValidate()
//    {
//        SetProgress(InspectorVal);
//    }

        [SerializeField, Range(0, 3f)] private float AnimationTime;

        [SerializeField] private bool LerpColor;

        [SerializeField] private Color emptyColor = Color.red;
        [SerializeField] private Color fullColor = Color.green;


        private readonly AnimationCurve smoothing = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Image image;

        private Image Image
        {
            get
            {
                if (!image)
                {
                    image = GetComponent<Image>();
                }

                return image;
            }
        }
        
        public void SetProgress(float progress)
        {
            StopAllCoroutines();
            if (gameObject.activeInHierarchy) StartCoroutine(SmoothValue(progress));
            else SetProgressImmediate(progress);
        }

        public void SetProgressImmediate(float value)
        {
            StopAllCoroutines();
            SetFill(value);
        }

        private IEnumerator SmoothValue(float target)
        {
            var startVal = Image.fillAmount;
            var elapsedTime = 0f;

            if (Mathf.Abs(target - startVal) < 0.01) yield break;

            while (elapsedTime < AnimationTime)
            {
                elapsedTime += Time.deltaTime;
                SetFill(Mathf.Lerp(startVal, target, smoothing.Evaluate(elapsedTime / AnimationTime)));

                yield return null;
            }

            SetFill(target);
        }

        private void SetFill(float value)
        {
            Image.fillAmount = value;

            if (LerpColor) Image.color = Color.Lerp(emptyColor, fullColor, value);
        }
    }
}