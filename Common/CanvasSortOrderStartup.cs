using UnityEngine;

namespace UE.Common
{
    /// <summary>
    /// This component sets the sorting order of the attached canvas to the given value at awake.
    /// Intented for prototyping.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CanvasSortOrderStartup : MonoBehaviour
    {
        [SerializeField] private int sortOrder = 0;

        void Awake()
        {
            GetComponent<Canvas>().sortingOrder = sortOrder;
        }
    }
}