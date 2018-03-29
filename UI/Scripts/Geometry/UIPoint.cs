using UE.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UE.UI.Geometry
{
    /// <summary>
    /// UI representation of a point.
    /// </summary>
    public class UIPoint : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image stroke1;
        [SerializeField] private Image stroke2;
        [SerializeField] private Text descriptionText;


#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
        
        [Header("Settings")]
        [SerializeField]
        private Vector2 position;
        [SerializeField]
        private Color color;
        [SerializeField]
        private string description;
        
#pragma warning restore 0168
#pragma warning restore 0219
#pragma warning restore 0414

        /// <summary>
        /// Sets the position of this point.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position)
        {
            this.position = position;
            transform.localPosition = position.XY0();
        }

        /// <summary>
        /// Sets the color of this point.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            this.color = color;
            stroke1.color = color;
            stroke2.color = color;
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
////            SetPosition(position);
//            SetDescription(description);
//        }
    }
}
