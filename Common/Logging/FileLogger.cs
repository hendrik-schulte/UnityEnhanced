using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Common
{
    /// <summary>
    /// This class enables easy logging to a file on disc. Automatically creates a StreamWriter
    /// object for every log file path.
    /// </summary>
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
                //Loading stream from cache
                return streams[fileName];
            }

            //Can't find the StreamWriter in the cache. Creating new one ...

            //Making sure the containing directory exists
            var directory = Path.GetDirectoryName(fileName);
            if (directory.Any()) Directory.CreateDirectory(directory);

            var stream = new StreamWriter(fileName, true);
            streams.Add(fileName, stream);
            stream.WriteLine("#### Started Logging on " + $"{DateTime.Now:F}" + " ####");
            return stream;
        }

        /// <summary>
        /// Closes the streams and clears the stream cache.
        /// </summary>
        public static void Close()
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

        /// <summary>
        /// Write a message to a log file based on the given log settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="message"></param>
        /// <param name="prefix">prefix added </param>
        public static void Write(LogToFile settings, string message, string prefix = "")
        {
            if (!settings.logToFile) return;

            if (prefix.Any())
            {
                var path = prefix + "_" + Path.GetFileName(settings.FileName);

                var directory = Path.GetDirectoryName(settings.FileName);

                if (directory.Any())
                    path = directory + "/" + path;

                Write(path, message);
            }
            else Write(settings.FileName, message);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws an Editor Gui for enabling the logger and defining the log file name.
        /// </summary>
        /// <param name="enabledField">Bool property defining if logging is enabled.</param>
        /// <param name="fileName">String property defining the path to the log file.</param>
        public static void LoggerControl(
            SerializedProperty enabledField,
            SerializedProperty fileName)
        {
            EditorGUILayout.PropertyField(enabledField);

            DrawLoggingSettings(enabledField, fileName, null, false);
        }

        public static void LoggerControl(
            SerializedProperty enabledField,
            SerializedProperty fileName,
            SerializedProperty separateLogsForInstance,
            bool instanced)
        {
            EditorGUILayout.PropertyField(enabledField);

            DrawLoggingSettings(enabledField, fileName, separateLogsForInstance, instanced);
        }

        private static void DrawLoggingSettings(
            SerializedProperty enabledField,
            SerializedProperty fileName,
            SerializedProperty separateLogsForInstance,
            bool instanced)
        {
            if (!enabledField.boolValue) return;

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(fileName);
            if (instanced && separateLogsForInstance != null)
                EditorGUILayout.PropertyField(separateLogsForInstance);

            EditorGUI.indentLevel--;
        }
#endif
    }
}