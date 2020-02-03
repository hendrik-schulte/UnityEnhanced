using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace UE.Common
{
    /// <summary>
    /// This utility class offers some extention methods for miscellaneous purposes.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Transform & GameObject

        /// <summary>
        /// Destroys all GameObjects in this List and clears the list afterwards.
        /// </summary>
        /// <param name="list"></param>
        public static void DestroyGOs(this List<GameObject> list)
        {
            foreach (var go in list)
            {
                Object.Destroy(go);
            }

            list.Clear();
        }

        /// <summary>
        /// Removes and destroys all GameObjects that are parented by this transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyChildren(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                Object.Destroy(transform.GetChild(0).gameObject);
                transform.GetChild(0).SetParent(null);
            }
        }

        #endregion

        #region UGUI

        /// <summary>
        /// Sets the distance to the left border in stretchable mode.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left"></param>
        public static void SetLeft(this RectTransform rect, float left)
        {
            rect.offsetMin = new Vector2(left, rect.offsetMin.y);
        }

        /// <summary>
        /// Sets the distance to the left border in stretchable mode.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="right"></param>
        public static void SetRight(this RectTransform rect, float right)
        {
            rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
        }


        /// <summary>
        /// Calculates the size of a vertical layout group based on its content elements.
        /// </summary>
        /// <returns></returns>
        public static Vector2 GetContentSize(this VerticalLayoutGroup layoutGroup, RectTransform contentParent)
        {
            float elementSpace = 0;

            for (int i = 0; i < contentParent.childCount; i++)
            {
                var element = contentParent.transform.GetChild(i);

                elementSpace += element.GetComponent<LayoutElement>().minHeight;
            }

            //apply spacing
            var uiFieldCount = contentParent.childCount;
            if (uiFieldCount > 1) elementSpace += layoutGroup.spacing * (uiFieldCount - 1);

            return new Vector2(contentParent.sizeDelta.x, elementSpace);
        }

        #endregion

        #region Vector3

        /// <summary>
        /// Returns the translation m of this Matrix4x4.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Vector3 GetTranslation(this Matrix4x4 m)
        {
            return m.GetColumn(3);
        }

        /// <summary>
        /// Returns the scale component from this Matrix4x4.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Vector3 GetScale(this Matrix4x4 m)
        {
            return new Vector3(
                m.GetColumn(0).magnitude,
                m.GetColumn(1).magnitude,
                m.GetColumn(2).magnitude
            );
        }

        /// <summary>
        /// Projects the given Vector2 onto the XY-Plane thus z = 0.
        /// </summary>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static Vector3 XY0(this Vector2 vec2)
        {
            return new Vector3(vec2.x, vec2.y, 0);
        }

        /// <summary>
        /// Projects the given Vector4 onto the XY-Plane thus z = 0.
        /// </summary>
        /// <param name="vec4"></param>
        /// <returns></returns>
        public static Vector3 XY0(this Vector4 vec4)
        {
            return new Vector3(vec4.x, vec4.y, 0);
        }

        /// <summary>
        /// Projects the given Vector2 onto the XZ-Plane thus y = 0.
        /// </summary>
        /// <param name="vec2"></param>
        /// <returns></returns>
        public static Vector3 X0Y(this Vector2 vec2)
        {
            return new Vector3(vec2.x, 0, vec2.y);
        }

        #endregion

        #region Matrix4x4

        /// <summary>
        /// Turns this matrix into a readable string wht the given format.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="format"></param>
        /// <param name="padding"></param>
        /// <returns></returns>
        public static string ToReadableString(this Matrix4x4 m, string format, string padding = " ")
        {
            return m[0, 0].ToString(format) + padding + m[0, 1].ToString(format) + padding + m[0, 2].ToString(format) +
                   padding + m[0, 3].ToString(format) + "\n"
                   + m[1, 0].ToString(format) + padding + m[1, 1].ToString(format) + padding +
                   m[1, 2].ToString(format) + padding + m[1, 3].ToString(format) + "\n"
                   + m[2, 0].ToString(format) + padding + m[2, 1].ToString(format) + padding +
                   m[2, 2].ToString(format) + padding + m[2, 3].ToString(format) + "\n"
                   + m[3, 0].ToString(format) + padding + m[3, 1].ToString(format) + padding +
                   m[3, 2].ToString(format) + padding + m[3, 3].ToString(format) + "\n";
        }


        /// <summary>
        /// Returns a Quaternion based on this Matrix4x4.
        /// Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Quaternion GetRotation(this Matrix4x4 m)
        {
            var q = new Quaternion();
            q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
            q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
            q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
            q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
            q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
            q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
            q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
            return q;
        }

        #endregion

        #region Collections

        /// <summary>
        /// Only adds the given element to the list if it is not contained in it yet.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddSingle<T>(this IList<T> list, T item)
        {
            if(!list.Contains(item))
                list.Add(item);
        }

        /// <summary>
        /// Only adds the given element to the set if it is not contained in it yet.
        /// </summary>
        /// <param name="set"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static void AddSingle<T>(this ISet<T> set, T item)
        {
            if(!set.Contains(item))
                set.Add(item);
        }
        
        /// <summary>
        /// Returns the first element that meets the given condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        public static T Find<T>(this IEnumerable<T> list, Func<T, bool> comparator)
        {
            return list.FirstOrDefault(comparator);
        }

        /// <summary>
        /// Returns the first occurence of the given type in the array. Returns null if there is no such field.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T FirstElementOfType<T, TR>(this IEnumerable<TR> array)
        {
            return array.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Returns a random element from this list based on UnityEngine.Random.Range(int,int).
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Returns a random index within the range of this list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int RandomIndex<T>(this List<T> list)
        {
            return Random.Range(0, list.Count);
        }

        /// <summary>
        /// Removes all elements from the list that return true on the given comparator function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparator"></param>
        public static void RemoveElements<T>(this List<T> list, Func<T, bool> comparator)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var element = list[i];

                if (comparator.Invoke(element)) list.RemoveAt(i);
            }
        }

        /// <summary>
        /// Returns a string with the list and its elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToStringElements<T>(this IEnumerable<T> list)
        {
            return list.ToStringElements(x => x != null ? x.ToString() : "null");
        }    

        /// <summary>
        /// Returns a string with the list and its elements using a custom string converter for each element.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="customStringConversion"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ToStringElements<T>(this IEnumerable<T> list, Func<T, string> customStringConversion)
        {
            var result = "{ ";

            var enumerable = list.ToList();
            
            result = enumerable.Aggregate(result, (current, item) => current + customStringConversion.Invoke(item) + ", ");

            if (enumerable.Any())
                result = result.TrimToSize(result.Length - 2) + " ";

            return result + "}";
        }


        /// <summary>
        /// Returns a string with the array and its elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ToStringElements<T>(this T[] array)
        {
            var result = "{ ";

            result = array.Aggregate(result, (current, item) => current + (item + ", "));

            if (array.Length > 0)
                result = result.TrimToSize(result.Length - 2) + " ";

            return result + "}";
        }

        /// <summary>
        /// Same as Where(x =&gt; x != null).
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TSource> WhereNotNull<TSource>(this IEnumerable<TSource> source)
        {
            return source.Where(x => x != null);
        }

        /// <summary>
        /// Allows inline execution over an enumeration similar to List.ForEach.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
                action.Invoke(item);
            return collection;
        }

        /// <summary>
        /// Starting from a given element in the list, returns the following element.
        /// If its the last element in the list, returns null.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="current"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetNext<T>(this IList<T> list, T current) where T : class
        {
            var index = list.IndexOf(current);

            if (index == -1)
                throw new ArgumentException("Element was not present in the list.");

            var newIndex = index + 1;

            return list.Count > newIndex ? list[newIndex] : null;
        }

        /// <summary>
        /// Same as <see cref="List{T}.AddRange"/> but works for ILists as well.
        /// </summary>
        /// <param name="iList"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRange<T>(this IList<T> iList, IEnumerable<T> items)
        {
            if (iList == null) throw new ArgumentNullException(nameof(iList));
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (iList is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    iList.Add(item);
                }
            }
        }
        
        /// <summary>
        /// Adds the given elements to the collection.
        /// </summary>
        /// <param name="iList"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddRange<T>(this IList<T> iList, params T[] items)
        {
            if (iList == null) throw new ArgumentNullException(nameof(iList));
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (iList is List<T> list)
            {
                list.AddRange(items);
            }
            else
            {
                foreach (var item in items)
                {
                    iList.Add(item);
                }
            }
        }

        /// <summary>
        /// Same as <see cref="List{T}.InsertRange"/> but works for ILists as well.
        /// </summary>
        /// <param name="iList"></param>
        /// <param name="insertPosition"></param>
        /// <param name="items"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public static void InsertRange<T>(this IList<T> iList, int insertPosition, IList<T> items)
        {
            if (iList == null) throw new ArgumentNullException(nameof(iList));
            if (items == null) throw new ArgumentNullException(nameof(items));

            if (iList is List<T> list)
            {
                list.InsertRange(insertPosition, items);
            }
            else
            {
                for (int i = items.Count - 1; i >= 0; i--)
                {
                    iList.Insert(insertPosition, items[i]);
                }
            }
        }

        /// <summary>
        /// Similarly to List.GetRange, this returns a number of elements starting from the given index.
        /// </summary>
        /// <param name="iList"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IList<T> GetRange<T>(this IList<T> iList, int index, int count)
        {
            switch (iList)
            {
                case null:
                    throw new ArgumentNullException("iList");
                case List<T> list:
                    return list.GetRange(index, count);
            }

            var result = new List<T>();

            for (int i = index; i < index + count; i++)
            {
                result.Add(iList[i]);
            }

            return result;
        }

        /// <summary>
        /// Removes all elements in the given collections from this list.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="remove"></param>
        /// <typeparam name="T"></typeparam>
        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> remove)
        {
            remove.ForEach(renderer => list.Remove(renderer));
        }

        /// <summary>
        /// Similarly to <see cref="List{T}.Sort()"/>, this sorts an IList in place.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="comparison"></param>
        /// <typeparam name="T"></typeparam>
        public static void Sort<T>(this IList<T> list, Comparison<T> comparison)
        {
            ArrayList.Adapter((IList) list).Sort(new ComparisonComparer<T>(comparison));
        }

        /// <summary>
        /// Convenience method on <see cref="IEnumerable{T}"/> to allow passing of a <see cref="Comparison{T}"/> delegate to
        /// the OrderBy method.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="comparison"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, Comparison<T> comparison)
        {
            return list.OrderBy(t => t, new ComparisonComparer<T>(comparison));
        }
        
        /// <summary>
        /// Wraps a generic <see cref="Comparison{T}"/> delegate in an <see cref="IComparer"/> to make it easy
        /// to use a lambda expression for methods that take an <see cref="IComparer"/> or <see cref="IComparer{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ComparisonComparer<T> : IComparer<T>, IComparer
        {
            private readonly Comparison<T> _comparison;

            public ComparisonComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return _comparison(x, y);
            }

            public int Compare(object o1, object o2)
            {
                return _comparison((T) o1, (T) o2);
            }
        }

        /// <summary>
        /// Iterates over the list providing an index starting at from (inclusive)
        /// and ending at to (exclusive).
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="to"></param>
        /// <param name="action"></param>
        /// <param name="from"></param>
        /// <typeparam name="T"></typeparam>
        public static void IterateRange<T>(this IList<T> ie, int from, int to, Action<(T item, int index)> action)
        {
            if (from <= 0) throw new ArgumentException("from must be greater or equal to 0");
            if (from >= to) throw new ArgumentException("from value must be smaller than to");

            for (int i = from; i < to; i++)
                action.Invoke((ie[i], i));
        }

        /// <summary>
        /// Iterates over the list providing an index starting at from (inclusive)
        /// and ending at to (exclusive).
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="to"></param>
        /// <param name="action"></param>
        /// <param name="from"></param>
        /// <typeparam name="T"></typeparam>
        public static void IterateRange<T>(this IReadOnlyList<T> ie, int from, int to, Action<(T item, int index)> action)
        {
            if (from <= 0) throw new ArgumentException("from must be greater or equal to 0");
            if (from >= to) throw new ArgumentException("from value must be smaller than to");

            for (int i = from; i < to; i++)
                action.Invoke((ie[i], i));
        }

        /// <summary>
        /// Iterates over the given enumeration providing an index (starting by 0).
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void ForEachIndex<T>(this IEnumerable<T> ie, Action<(T item, int index)> action)
        {
            var i = 0;
            foreach (var e in ie) action((e, i++));
        }
        
        /// <summary>
        /// Iterates over the given enumeration returning the index of the given
        /// element (or -1 of it is not contained in the collection).
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        public static int IndexOf<T>(this IEnumerable<T> ie, T item)
        {
            var i = 0;
            foreach (var e in ie)
            {
                if (item.Equals(e)) return i;
                i++;
            }
            
            return -1;
        }

        #endregion

        #region Layer & LayerMask

        /// <summary>
        /// Recursively sets the layers of all children of this game object to the given layer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="layer"></param>
        public static void SetLayerRecursively(this GameObject obj, int layer, bool onlyChangeDefault = true)
        {
            if (!obj)
                return;

            if ( !onlyChangeDefault || (onlyChangeDefault && obj.layer == 0) )
            {
                obj.layer = layer;
            }
            

            foreach (Transform child in obj.transform)
            {
                if (child)
                {
                    SetLayerRecursively(child.gameObject, layer);
                }
            }
        }

        /// <summary>
        /// Returns the hierarchy above this transform as a string.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static string GetTransformHierarchy(this Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }

            return path;
        }

        /// <summary>
        /// Returns the hierarchy above this GameObject as a string.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static string GetTransformHierarchy(this GameObject gameObject)
        {
            return gameObject.transform.GetTransformHierarchy();
        }

        /// <summary>
        /// Returns true if the layer is contained in this layermask.
        /// </summary>
        /// <param name="layerMask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        #endregion

        #region Types & Attributes

        /// <summary>
        /// Returns the first Attribute of the given Type that is assigned to this type.
        /// Returns null if there is none.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetFirstAttributeOfType<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(true).FirstElementOfType<T, object>();
        }

        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Returns true if this type is a subclass of the given generic type. 
        /// </summary>
        /// <param name="derived"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfGeneric(this Type derived, Type generic)
        {
            if (derived == generic)
                return false;

            if (derived.IsSubclassOf(generic))
                return true;

            var parameters = generic.GetGenericArguments();
            var isParameterLessGeneric = !(parameters.Length > 0 &&
                                           ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) ==
                                            TypeAttributes.BeforeFieldInit));

            while (derived != null && derived != typeof(object))
            {
                var cur = GetFullTypeDefinition(derived);
                if (generic == cur || (isParameterLessGeneric && cur.GetInterfaces()
                                           .Select(GetFullTypeDefinition)
                                           .Contains(GetFullTypeDefinition(generic))))
                    return true;
                else if (!isParameterLessGeneric)
                    if (GetFullTypeDefinition(generic) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(generic), cur))
                            if (VerifyGenericArguments(generic, derived))
                                return true;
                    }
                    else
                        foreach (var item in derived.GetInterfaces()
                            .Where(i => GetFullTypeDefinition(generic) == GetFullTypeDefinition(i)))
                            if (VerifyGenericArguments(generic, item))
                                return true;

                derived = derived.BaseType;
            }

            return false;
        }

        private static Type GetFullTypeDefinition(Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static bool VerifyGenericArguments(Type parent, Type child)
        {
            Type[] childArguments = child.GetGenericArguments();
            Type[] parentArguments = parent.GetGenericArguments();
            if (childArguments.Length == parentArguments.Length)
                for (int i = 0; i < childArguments.Length; i++)
                    if (childArguments[i].Assembly != parentArguments[i].Assembly ||
                        childArguments[i].Name != parentArguments[i].Name ||
                        childArguments[i].Namespace != parentArguments[i].Namespace)
                        if (!childArguments[i].IsSubclassOf(parentArguments[i]))
                            return false;

            return true;
        }

        #endregion

        public static string ConvertAssetToResourcesPath(this string assetPath)
        {
            int position = assetPath.LastIndexOf("Resources/", StringComparison.Ordinal);

            var woPath = assetPath.Substring(position + 10);

            return woPath.Substring(0, woPath.Length - 7);
        }

        /// <summary>
        /// Removes the given amount of characters from the end of this string.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string TrimFromEnd(this string text, int size)
        {
            return text.Substring(0, Math.Max(text.Length - size, 0));
        }


        /// <summary>
        /// Trims the string to the given length only taking the first characters.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string TrimToSize(this string text, int size)
        {
            if (text.Length < size) return text;

            return text.Substring(0, size);
        }

        /// <summary>
        /// Savely converts a string to an integer.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int StringToInt(this string value)
        {
            int result;

            try
            {
                result = (int) Convert.ChangeType(value, typeof(int), CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (FormatException)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Savely converts a string to a float.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float StringToFloat(this string value)
        {
            float result;

            try
            {
                result = (float) Convert.ChangeType(value, typeof(float), CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (FormatException)
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Maps this float to another range keeping the relative distance between the bounds.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="fromMin"></param>
        /// <param name="fromMax"></param>
        /// <param name="toMin"></param>
        /// <param name="toMax"></param>
        /// <returns></returns>
        public static float MapRange(this float val, float fromMin, float fromMax, float toMin, float toMax)
        {
            return toMin + (val - fromMin) * (toMax - toMin) / (fromMax - fromMin);
        }

#if UNITY_EDITOR

        #region Editor Only

        /// <summary>
        /// Returns a list of the SerializedProperties contained in this SerializedObject.
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <returns></returns>
        public static List<SerializedProperty> ToList(this SerializedObject serializedObject)
        {
            var iterator = serializedObject.GetIterator();
            iterator.Next(true);

            var result = new List<SerializedProperty>();
            
            foreach (SerializedProperty obj in iterator)
            {
//                Logging.Log(typeof(ProjectSettingsEnforcer), $"found {obj.name}");
                result.Add(obj);
            }

            return result;
        }

        #endregion
        
        
#endif
    }
}