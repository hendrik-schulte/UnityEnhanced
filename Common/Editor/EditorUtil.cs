﻿#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UE.Common.SubjectNerd.Utilities;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

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
        /// Returns an offset version of this Rect where positive values enlarge the rect and vice versa.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left">Positive value enlarges the rectangle to the left.</param>
        /// <param name="right">Positive value enlarges the rectangle to the right.</param>
        /// <param name="top">Positive value enlarges the rectangle to the top.</param>
        /// <param name="bottom">Positive value enlarges the rectangle to the bottom.</param>
        /// <returns></returns>
        public static Rect Offset(this Rect rect, int left, int right, int top, int bottom)
        {
            var result = new Rect(rect);

            result.xMin -= left;
            result.xMax += right;
            result.yMin -= top;
            result.yMax += bottom;

            return result;
        }

        /// <summary>
        /// Returns an offset version of this Rect where positive values enlarge the rect and vice versa.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left">Positive value enlarges the rectangle to the left.</param>
        /// <param name="right">Positive value enlarges the rectangle to the right.</param>
        /// <param name="top">Positive value enlarges the rectangle to the top.</param>
        /// <param name="bottom">Positive value enlarges the rectangle to the bottom.</param>
        /// <returns></returns>
        public static Rect Offset(this Rect rect, float left, float right, float top, float bottom)
        {
            return rect.Offset((int) left, (int) right, (int) top, (int) bottom);
        }


        /// <summary>
        /// This splits the position Rect of a Property Drawer to single lines given the row number
        /// (starting by 1). Works nicely when the Property height is calculated using
        /// EditorUtil.PropertyHeight().
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
        /// This splits the position Rect of a Property Drawer to single lines given the row number
        /// (starting by 1). Works nicely when the Property height is calculated using
        /// EditorUtil.PropertyHeight(). This overload automatically increments the line after the call.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="line">This automatically gets incremented after the line is drawn.</param>
        /// <returns></returns>
        public static Rect GetLine(this Rect position, ref int line)
        {
            return position.GetLine(line++);
        }


        /// <summary>
        /// Splits this rectangle into a column based on the column number (from 1 to totalColumns)
        /// and the total number of columns.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="column"></param>
        /// <param name="totalColumns"></param>
        /// <returns></returns>
        public static Rect Column(this Rect position, int column, int totalColumns, int spacing = 1)
        {
            var columnsWidth = position.width / totalColumns;

            return new Rect(position.x + ((column - 1) * columnsWidth),
                    position.y, columnsWidth, position.height)
                .Offset(-spacing, -spacing, 0, 0);
        }

        /// <summary>
        /// Returns columns inside this rectangle.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="column"></param>
        /// <param name="numColumns"></param>
        /// <param name="totalColumns"></param>
        /// <returns></returns>
        public static Rect Columns(this Rect position, int column, int numColumns, int totalColumns, int spacing = 1)
        {
            var columnsWidth = position.width / totalColumns;

            return new Rect(position.x + ((column - 1) * columnsWidth),
                    position.y, columnsWidth * numColumns, position.height)
                .Offset(-spacing, -spacing, 0, 0);
        }

        /// <summary>
        /// Similar to <see cref="GetLine(UnityEngine.Rect,int)"/> but returns
        /// a number of lines starting from the given line.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="line"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static Rect GetLines(this Rect position, int line, int num)
        {
            var rect = position.GetLine(line);

            if (num > 1)
                rect.yMax += (num - 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

            return rect;
        }

        /// <summary>
        /// Similar to <see cref="GetLine(UnityEngine.Rect,int, ref int)"/> but returns
        /// a number of lines starting from the given line.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="line"></param>
        /// <param name="num"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public static Rect GetLines(this Rect position, ref int line, int num)
        {
            var rect = position.GetLines(line, num);

            line += num;
            
            return rect;
        }


//        public static float GetTotalHeight(int lines)
//        {
//            var height = EditorGUIUtility.singleLineHeight;
//            
//            if (lines > 1)
//            {
//                height += (lines - 1) * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
//            }
//            
//            return height;
//        }

        /// <summary>
        /// Returns the first occourence of the given attribute
        /// on this property or null if there is none.
        /// </summary>
        /// <param name="property"></param>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <returns></returns>
        public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            var attributes = property.GetAttributes<T>();

            if (!attributes.Any()) return null;

            return attributes[0] as T;
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

        /// <summary>
        /// Returns the color of the Editor.
        /// </summary>
        public static Color EditorBackgroundColor => EditorGUIUtility.isProSkin
            ? new Color32(56, 56, 56, 255)
            : new Color32(194, 194, 194, 255);

        /// <summary>
        /// Instantiates a prefab asset when alt-clicking it.
        /// </summary>
        /// <see cref="https://www.reddit.com/r/Unity3D/comments/8upjn3/i_discovered_the_onopenasset_attribute_and_made_a/"/>
        /// <param name="instanceID"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset]
        private static bool HandleOpenEvent(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);

            var prefab = asset as GameObject;
            if (prefab != null)
                return OnOpenPrefab(prefab);

            return false;
        }

        private static bool OnOpenPrefab(GameObject prefab)
        {
            var current = Event.current;

            if (current.alt) // Alt to Instantiate.
            {
                var instance = PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(instance, "Alt Instantiate");
                (instance as GameObject).transform.SetAsLastSibling();
                Selection.activeObject = instance;
                EditorGUIUtility.PingObject(instance);

                if (current.control) // Ctrl to frame the instance in the scene view.
                {
                    if (SceneView.lastActiveSceneView != null)
                    {
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                }

                return true;
            }

            return false;
        }
    }
}
#endif