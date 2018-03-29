using UE.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UE.UI.Geometry
{
    /// <summary>
    /// This draws a 2D line from one point to annother.
    /// </summary>
    public class UILine : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private RectTransform line;

        [SerializeField] private Image lineImage;
        [SerializeField] private RectTransform fromRect;
        [SerializeField] private Image fromImage;
        [SerializeField] private RectTransform toRect;
        [SerializeField] private Image toImage;
        [SerializeField] private RectTransform descriptionRect;
        [SerializeField] private Text descriptionText;

        [Header("Settings")]
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
    
        [SerializeField]
        private Vector2 from;
        [SerializeField] 
        private Vector2 to;
        
        [SerializeField] private Color color = Color.black;
        [SerializeField] private string description;
        
#pragma warning restore 0168
#pragma warning restore 0219
#pragma warning restore 0414

        [SerializeField] [Range(0, 1)] private float lineThickness = 0.066f;
        [SerializeField] [Range(1, 10)] private float endPointThickness = 1;


        private Vector2 vector;

        private void Start()
        {
            DrawLine();
        }

        public void SetFrom(Vector2 from)
        {
            SetFromTo(from, to);
        }

        public void SetTo(Vector2 to)
        {
            this.to = to;
            vector = to - from;
            DrawLine();    
        }

        /// <summary>
        /// Defines this vector by two points.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetFromTo(Vector2 from, Vector2 to)
        {
            this.from = from;
            transform.localPosition = from.XY0();
            SetTo(to);
        }

        /// <summary>
        /// Sets the color of this vector.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            this.color = lineImage.color = fromImage.color = toImage.color = color;
        }

        /// <summary>
        /// Sets the description of this vector.
        /// </summary>
        /// <param name="description"></param>
        public void SetDescription(string description)
        {
            this.description = description;
            descriptionText.text = description;
        }

//        private void OnValidate()
//        {
//            SetColor(color);
////            SetFromTo(from, to);
//            SetDescription(description);
//        }

        /// <summary>
        /// Redraws the vector.
        /// </summary>
        private void DrawLine()
        {
            var magnitude = vector.magnitude;

            if (magnitude <= 0.1)
            {
                lineImage.enabled = fromImage.enabled = toImage.enabled = descriptionText.enabled = false;
                return;
            }
            else
            {
                lineImage.enabled = fromImage.enabled = toImage.enabled = descriptionText.enabled = true;
            }

            //line position and rotation
            line.sizeDelta = new Vector2(magnitude / line.lossyScale.x, lineThickness);
            line.localPosition = Vector3.zero; //disabled because of unity warnings
            line.pivot = new Vector2(0, 0.5f);
            var angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            line.rotation = Quaternion.Euler(0, 0, angle);

            //description position
            descriptionRect.localPosition = vector * 0.5f;

            //from and to arrows position
            fromRect.sizeDelta = toRect.sizeDelta = new Vector2(lineThickness * endPointThickness, lineThickness * endPointThickness);
            toRect.localPosition = vector;
        }
    }
}