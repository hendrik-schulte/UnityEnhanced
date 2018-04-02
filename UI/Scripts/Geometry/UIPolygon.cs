using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.UI.Geometry
{
    /// <summary>
    /// This draws a set of points as a 2D Polygon.
    /// </summary>
    public class UIPolygon : MonoBehaviour
    {
        [SerializeField] private UILine linePrefab;
        [SerializeField] private Color color = Color.black;
        [SerializeField] private List<Vector2> points;

        private List<UILine> lines;

        /// <summary>
        /// Defines the points of this polygon and redraws it.
        /// </summary>
        /// <param name="points"></param>
        public void Set(IEnumerable<Vector2> points)
        {
            this.points = new List<Vector2>(points);
            Build();
        }

        /// <summary>
        /// Returns the points of this polygon.
        /// </summary>
        /// <returns></returns>        
        public ReadOnlyCollection<Vector2> Points => points.AsReadOnly();

        /// <summary>
        /// Sets the color of the polygon.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            this.color = color;

            foreach (var line in lines)
            {
                line.SetColor(color);
            }
        }

        /// <summary>
        /// Removes the current polygon lines.
        /// </summary>
        public void Clear()
        {
            if (lines == null)
            {
                lines = new List<UILine>();
                return;
            }

            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                var go = transform.GetChild(i).gameObject;

                if (!Application.isPlaying && Application.isEditor)
                    DestroyImmediate(go);
                else
                    Destroy(go);
            }

            lines.Clear();
        }

        /// <summary>
        /// Constructs the polygon based on the List of points defined.
        /// </summary>
        public void Build()
        {
            Clear();

            if (points.Count < 2) return;

            UILine line = null;

            for (var i = 0; i < points.Count; i++)
            {
                var point = points[i];

                if (i > 0) line.SetTo(point);

                line = Instantiate(linePrefab, transform);
                line.SetColor(color);
                line.SetDescription("");
                line.SetFrom(point);
                line.gameObject.layer = gameObject.layer;
                lines.Add(line);
            }

            line.SetTo(points[0]);
        }

        private void Start()
        {
            Build();
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(UIPolygon))]
    public class UIPolygonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var polygon = target as UIPolygon;

            if (GUILayout.Button("Build"))
                polygon.Build();


            if (GUILayout.Button("Clear"))
                polygon.Clear();
        }
    }
#endif
}