using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UE.Common
{
    /// <summary>
    /// This utility class offers some extention methods for miscellaneous purposes.
    /// </summary>
    public static class ExtensionMethods
    {
        public static string ConvertAssetToResourcesPath(this string assetPath)
        {
            int position = assetPath.LastIndexOf("Resources/");

            var woPath = assetPath.Substring(position + 10);

            return woPath.Substring(0, woPath.Length - 7);
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
        /// Destroys all GameObjects in this List and clears the list afterwards.
        /// </summary>
        /// <param name="list"></param>
        public static void DestroyGOs(this List<GameObject> list)
        {
//            if(list == null) Debug.LogError("ist is null");

            foreach (var go in list)
            {
                GameObject.Destroy(go);
            }

            list.Clear();
        }

        /// <summary>
        /// Returns the first element that meets the given condition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparator"></param>
        /// <returns></returns>
        public static T Find<T>(this List<T> list, Comparator<T> comparator) where T : class
        {
            foreach (var go in list)
            {
                if (comparator(go)) return go;
            }

            return null;
        }

        /// <summary>
        /// Returns the first ocourence of the given type in the array. Returns null if there is no such field.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T FirstElementOfType<T>(this object[] array) where T : class
        {
            for (int index = 0; index < array.Length; index++)
            {
                var e = array[index];

                if (e is T)
                {
                    return (T) e;
                }
            }

            return null;
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

        /// <summary>
        /// Sets the distance to the left border in strechable mode.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="left"></param>
        public static void SetLeft(this RectTransform rect, float left)
        {
            rect.offsetMin = new Vector2(left, rect.offsetMin.y);
        }

        /// <summary>
        /// Sets the distance to the left border in strechable mode.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="right"></param>
        public static void SetRight(this RectTransform rect, float right)
        {
            rect.offsetMax = new Vector2(-right, rect.offsetMax.y);
        }

        /// <summary>
        /// This function descides if an element is the on you look for in the list. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns>True when it is the left element.</returns>
        public delegate bool Comparator<T>(T element);

        /// <summary>
        /// Removes all elements from the list that return true on the given comparator function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparator"></param>
        public static void RemoveElements<T>(this List<T> list, Comparator<T> comparator)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var element = list[i];

                if (comparator(element)) list.RemoveAt(i);
            }
        }

        /// <summary>
        /// Removes and destroys all GameObjects that are parented by this transform.
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyChildren(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                GameObject.Destroy(transform.GetChild(0).gameObject);
                transform.GetChild(0).SetParent(null);
            }
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
        /// Returns the first Attribute of the given Type that is assigned to this type.
        /// Returns null if there is none.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetFirstAttributeOfType<T>(this Type type) where T : Attribute
        {
            return type.GetCustomAttributes(true).FirstElementOfType<T>();
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
        /// Turns this matrix into a readable formatable string.
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
        /// Returns a string with the list and its elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToStringElements<T>(this List<T> list)
        {
            var result = list + ": { ";

            foreach (var item in list)
            {
                result += item + ", ";
            }

            return result + "}";
        }

        /// <summary>
        /// Recursively sets the layers of all children of this game object to the given layer.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="_layer"></param>
        public static void SetLayerRecursively(this GameObject obj, int _layer)
        {
            if (!obj)
                return;

            obj.layer = _layer;

            foreach (Transform child in obj.transform)
            {
                if (child)
                {
                    SetLayerRecursively(child.gameObject, _layer);
                }
            }
        }

        /// <summary>
        /// Returns the hierachy above this transform as a string.
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
        /// Returns the hierachy above this transform as a string.
        /// </summary>
        /// <param name="transform"></param>
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
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static bool IsSubClassOfGeneric(this Type child, Type parent)
        {
            if (child == parent)
                return false;

            if (child.IsSubclassOf(parent))
                return true;

            var parameters = parent.GetGenericArguments();
            var isParameterLessGeneric = !(parameters != null && parameters.Length > 0 &&
                                           ((parameters[0].Attributes & TypeAttributes.BeforeFieldInit) ==
                                            TypeAttributes.BeforeFieldInit));

            while (child != null && child != typeof(object))
            {
                var cur = GetFullTypeDefinition(child);
                if (parent == cur || (isParameterLessGeneric && cur.GetInterfaces()
                                          .Select(i => GetFullTypeDefinition(i))
                                          .Contains(GetFullTypeDefinition(parent))))
                    return true;
                else if (!isParameterLessGeneric)
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                            if (VerifyGenericArguments(parent, child))
                                return true;
                    }
                    else
                        foreach (var item in child.GetInterfaces()
                            .Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i)))
                            if (VerifyGenericArguments(parent, item))
                                return true;

                child = child.BaseType;
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
    }
}