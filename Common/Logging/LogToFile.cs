using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UE.Common
{
    /// <summary>
    /// This wraps all settings for file logging.
    /// </summary>
    [Serializable]
    public class LogToFile
    {
        [Tooltip("Enables automatic logging to a file. You need to take care of closing the FileStreams created by" +
                 "this by adding a FileStreamCloser component anywhere in your scene.")]
        public bool logToFile;

        [Tooltip("Name of the log file.")] public string FileName = "Logs/main.log";
    }
}