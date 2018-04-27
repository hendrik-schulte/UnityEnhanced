using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UE.Common
{
    public static class FileLogger
    {
        /// <summary>
        /// This is the Resources cache.
        /// </summary>
        private static readonly Dictionary<string, StreamWriter> streams = new Dictionary<string, StreamWriter>();

        /// <summary>
        /// Returns a StreamWriter object that writes to the given file. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static StreamWriter GetStreamWriter(string fileName)
        {
            if (streams.ContainsKey(fileName))
            {
                //Loading stream from dictionary
                return streams[fileName];
            }

            //Can't find the StreamWriter in the cache. Creating new one ...
            //Making sure the containing directory exists
            var directory = Path.GetDirectoryName(fileName);
            if(directory.Any()) Directory.CreateDirectory(directory);
            
            var stream = new StreamWriter(fileName, true);
            streams.Add(fileName, stream);
//            Write(fileName, "Started Logging on " + $"[{DateTime.Now:D}]");
            stream.WriteLine("#### Started Logging on " + $"{DateTime.Now:F}" + " ####");
            return stream;
        }
        
        /// <summary>
        /// Closes the streams and clears the stream cache.
        /// </summary>
        public static void Flush()
        {
            foreach (var stream in streams)
            {
                stream.Value.Close();
            }

            streams.Clear();
        }

        /// <summary>
        /// Writes a message to the given log file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="message"></param>
        public static void Write(string fileName, string message)
        {
            var now = DateTime.Now;
            message = $"[{now:H:mm:ss}] {message}";

            var stream = GetStreamWriter(fileName);
            stream.WriteLine(message);
            stream.Flush();
        }
        
#if UNITY_EDITOR
        public static void LoggerControl(SerializedProperty enabledField, SerializedProperty fileName)
        {
            EditorGUILayout.PropertyField(enabledField);

            if (!enabledField.boolValue) return;
            
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(fileName);
                
            EditorGUI.indentLevel--;
        }
#endif
    }
}