using UE.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UE.UI.Geometry
{
    /// <summary>
    /// UI representation of a 2D vector.
    /// </summary>
    public class UIVector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private RectTransform line;
        [SerializeField] private Image lineImage;
        [SerializeField] private RectTransform arrow;
        [SerializeField] private Image arrowImage;
        [SerializeField] private RectTransform descriptionRect;
        [SerializeField] private Text descriptionText;

        [Header("Settings")]
        [SerializeField]
#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414

        private Vector2 position;
        [SerializeField]
        private Color color;
        [SerializeField]
        private string description;

#pragma warning restore 0168
#pragma warning restore 0219
#pragma warning restore 0414

        [SerializeField]
        private Vector2 vector;
        [SerializeField]
        [Range(0, 1)]
        private float vectorThickness;

        private void Start()
        {
            DrawLine();
        }

        /// <summary>
        /// Defines this vector by two points.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetFromTo(Vector2 from, Vector2 to)
        {
            Set(from, to - from);
        }

        /// <summary>
        /// Defines this vector by a starting point and direction.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="vector"></param>
        public void Set(Vector2 position, Vector2 vector)
        {
            this.position = position;
            transform.localPosition = position.XY0();
            this.vector = vector;
            DrawLine();
        }

        /// <summary>
        /// Moves this vector to the given position maintaining its direction.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
            transform.localPosition = position.XY0();
            DrawLine();
        }

        /// <summary>
        /// Sets this vectors direction by maintaining its origin.
        /// </summary>
        /// <param name="vector"></param>
        public void SetVector(Vector2 vector)
        {
            this.vector = vector;
            DrawLine();
        }

        /// <summary>
        /// Sets the color of this vector.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            this.color = color;
            lineImage.color = color;
            arrowImage.color = color;
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
////        Set(position, vector);
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
                lineImage.enabled = arrowImage.enabled = descriptionText.enabled = false;
                return;
            }
            else
            {
                lineImage.enabled = arrowImage.enabled = descriptionText.enabled = true;
            }

            //line position and rotation
            line.sizeDelta = new Vector2(magnitude / line.lossyScale.x, vectorThickness);
            line.localPosition = Vector3.zero; //disabled because of unity warnings
            line.pivot = new Vector2(0, 0.5f);
            var angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            line.rotation = Quaternion.Euler(0, 0, angle);
            //arrow position and rotation
            arrow.localPosition = vector;//disabled because of unity warnings
            arrow.rotation = Quaternion.Euler(0, 0, angle + 180);
            //description position
            descriptionRect.localPosition = vector * 0.5f;
        }
    }
}
