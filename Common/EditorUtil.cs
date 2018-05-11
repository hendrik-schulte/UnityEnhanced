#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UE.Common
{
    /// <summary>
    /// This class offers some utility functions for drawing custom editors.
    /// </summary>
    public static class EditorUtil
    {
        /// <summary>
        /// This calculates the height of a custom property based on the number of lines.
        /// Assumes every line to have the default height.
        /// </summary>
        /// <param name="numLines"></param>
        /// <returns></returns>
        public static float PropertyHeight(int numLines)
        {
            var spacing = 0;

            if (numLines > 1)
            {
                spacing = (numLines - 1) * (int) EditorGUIUtility.standardVerticalSpacing;
            }

            return numLines * EditorGUIUtility.singleLineHeight + spacing;
        }


        /// <summary>
        /// Returns a new child rectangle that lives inside this rectangle.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="yPos"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Rect GetSubRect(this Rect position, float yPos, float height)
        {
            return new Rect(position)
            {
                y = position.y + yPos,
                height = height
            };
        }

        /// <summary>
        /// This splits the position Rect of a Property Drawer to single lines given the row number.
        /// Works nicely when the Property height is calculated using EditorUtil.PropertyHeight().
        /// </summary>
        /// <param name="position"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public static Rect GetLine(this Rect position, int line)
        {
            var y = 0f;

            if (line > 1)
            {
                y = (line - 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }

            return position.GetSubRect(y, EditorGUIUtility.singleLineHeight);
        }

        /// <summary>
        /// Casts the given property to a list.
        /// Taken from https://answers.unity.com/questions/682932/using-generic-list-with-serializedproperty-inspect.html
        /// </summary>
        /// <param name="property"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ToList<T>(this SerializedProperty property) where T : Object
        {
            var sp = property.Copy(); // copy so we don't iterate the original

            sp.Next(true); // skip generic field
            sp.Next(true); // advance to array size field

            // Get the array size
            var arrayLength = sp.intValue;

            sp.Next(true); // advance to first array index

            // Write values to list
            var values = new List<T>(arrayLength);

            var lastIndex = arrayLength - 1;
            for (var i = 0;
                i < arrayLength;
                i++)
            {
                values.Add((T) sp.objectReferenceValue); // copy the value to the list
                if (i < lastIndex) sp.Next(false); // advance without drilling into children
            }

            return values;
        }
    }
}
#endif