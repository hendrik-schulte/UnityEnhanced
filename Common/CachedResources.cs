using System.Collections.Generic;
using UnityEngine;

namespace UE.Common
{
    /// <summary>
    /// This class allows automatic caching of resources to save performance in reoccouring
    /// Resources.Load calls. Just use CachedResources.Load<> as you would normally use Resources.Load<>.
    /// </summary>
    public static class CachedResources
    {
        /// <summary>
        /// This is the Resources cache.
        /// </summary>
        private static readonly Dictionary<string, Object> cache = new Dictionary<string, Object>();

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public static void Flush()
        {
            cache.Clear();
        }

        /// <summary>
        /// Attempts to find the asset in the cache. When not found,
        /// it tries to load it from a Resources folder and caches it.
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string path) where T : Object
        {
            var success = false;
            return Load<T>(path, out success);
        }
        
        /// <summary>
        /// Attempts to find the asset in the cache. When not found,
        /// it tries to load it from a Resources folder and caches it.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="path"></param>
        /// <param name="success"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Load<T>(string path, out bool success) where T : Object
        {
            T asset;

            if (cache.ContainsKey(path))
            {
                //Loading asset from dictionary.");
                asset = (T) cache[path];
            }
            else
            {
                //Can't find the asset in the cache. Calling Resources.Load ...
                asset = Resources.Load<T>(path);
                cache.Add(path, asset);
            }

            if (!asset)
            {
                Logging.Error("CachedResources", "The desired asset named '" + path + "' does not exist.");
                success = false;
                return null;
            }
            else
            {
                success = true;
                return asset;
            }
        }
    }
}