using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = System.Object;

namespace UE.Serialization
{
    /// <summary>
    /// This class offers different saving and loading options using BinaryFormater.
    /// Adapted from this: http://answers.unity3d.com/questions/8480/how-to-scrip-a-saveload-game-option.html
    /// and this: http://stackoverflow.com/questions/129389/how-do-you-do-a-deep-copy-an-object-in-net-c-specifically
    /// </summary>
    public class SaveLoad
    {
        /// <summary>
        /// Returns the default path where this class stores files when no absolute directory is specified. On desktop
        /// builds it will be next to the executable while in WebGL, it will save in a local database. In Editor it saves
        /// to "Assets/".
        /// </summary>
        /// <returns></returns>
        public static string DefaultPath()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return Application.persistentDataPath + "/Save/";
#else
            return Application.dataPath + "/Save/";
#endif
        }

        /// <summary>
        /// Saves the given object in a file with the given name. The object must be serializable.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void Save(string fileName, Object data)
        {
            Directory.CreateDirectory(DefaultPath());

            string directory = Path.GetDirectoryName(fileName);

            if (directory.Length > 0)
            {
                Directory.CreateDirectory(DefaultPath() + directory);
            }
            //Debug.Log("Saved " + fileName + ". Path: " + directory);

            Stream stream = File.Open(DefaultPath() + fileName, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Binder = new VersionDeserializationBinder();
            bformatter.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        /// Loads an object from the given file and deserializes it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T Load<T>(string fileName) where T : new()
        {
            T data = new T();

            string path = DefaultPath() + fileName;

            //Debug.Log("Loading from " + path);

            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException();

                Stream stream = File.Open(path, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Binder = new VersionDeserializationBinder();
                data = (T) bformatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("ERROR: Coudn't find file at path: " + path);
            }
            catch (DirectoryNotFoundException)
            {
                Debug.LogError("ERROR: Coudn't find directory at path: " + path);
            }

            if (data is IFile)
            {
                (data as IFile).SetPath(fileName, Path.GetFileNameWithoutExtension(DefaultPath() + fileName));
            }

            return data;
        }

        public static T LoadFromPath<T>(string pathAndFileName) where T : new()
        {
            T data = new T();
            Stream stream = File.Open(pathAndFileName, FileMode.Open);
            BinaryFormatter bformatter = new BinaryFormatter();
            bformatter.Binder = new VersionDeserializationBinder();
            data = (T) bformatter.Deserialize(stream);
            stream.Close();

            return data;
        }

        /// <summary>
        /// Creates a deep copy of the given object using serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T) formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Loads all files in the given directory with the given file extention and returns them as a list of the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="directory"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static List<T> LoadList<T>(string directory, string extention) where T : new()
        {
            var result = new List<T>();

            //string path = DefaultPath();

            //string[] files = Directory.GetFiles(path + directory, "*." + extention);

            foreach (var file in GetFilesByExtention(directory, extention))
            {
                result.Add(Load<T>(file));
            }

            return result;
        }

        /// <summary>
        /// Returns a list of paths to files in the given directory with the given file extention.
        /// For example: GetFilesByExtention("Replay", "rpl")
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="extention"></param>
        /// <returns></returns>
        public static List<string> GetFilesByExtention(string directory, string extention)
        {
            var result = new List<string>();

            string path = DefaultPath();

            string[] files;

            try
            {
                if (!Directory.Exists(path + directory))
                    throw new DirectoryNotFoundException();

                files = Directory.GetFiles(path + directory, "*." + extention);
            }
            catch (DirectoryNotFoundException)
            {
                Debug.Log("Warning: The directory  '" + path + directory + "' to load from does not exist.");
                files = new string[0];
            }

            foreach (var file in files)
            {
                result.Add(file.Substring(path.Length, file.Length - path.Length));
            }

            return result;
        }

        /// <summary>
        /// Serializes the given object to a byte array.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SerializeToByteArray(Object data)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, data);
            return stream.GetBuffer();
        }

        /// <summary>
        /// Deserializes the given byte array back to the initial object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteData"></param>
        /// <returns></returns>
        public static T DeserializeByteArray<T>(byte[] byteData)
        {
            MemoryStream stream = new MemoryStream(byteData);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (T) binaryFormatter.Deserialize(stream);
        }
    }

// === This is required to guarantee a fixed serialization assembly name, which Unity likes to randomize on each compile
// Do not change this
    public sealed class VersionDeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
            {
                Type typeToDeserialize = null;

                assemblyName = Assembly.GetExecutingAssembly().FullName;

                // The following line of code returns the type. 
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

                return typeToDeserialize;
            }

            return null;
        }
    }
}