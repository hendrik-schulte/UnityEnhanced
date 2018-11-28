using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UE.Common
{
    /// <summary>
    /// This class offers functions for console logging in a unified style. Automatically adds
    /// Type and name identifiers to each message.
    /// </summary>
    public static class Logging
    {
        public static void Log(Type sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(TypeIdentifier(sender) + msg);
        }
        
        public static void Log(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(TypeIdentifier(sender) + msg);
        }
        
        public static void Log(Object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(TypeIdentifier(sender) + Apostrophe(sender.name) + msg, sender);
        }
        
        public static void Log(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(Brackets(sender) + msg);
        }
        
        public static void Warning(Type sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(TypeIdentifier(sender) + msg);
        }

        public static void Warning(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(TypeIdentifier(sender) + msg);
        }
        
        public static void Warning(Object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(TypeIdentifier(sender) + Apostrophe(sender.name) + msg, sender);
        }

        public static void Warning(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(Brackets(sender) + msg);
        }
        
        public static void Error(Type sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(TypeIdentifier(sender) + msg);
        }

        public static void Error(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(TypeIdentifier(sender) + msg);
        }
        
        public static void Error(Object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(TypeIdentifier(sender) + Apostrophe(sender.name) + msg, sender);
        }
        
        public static void Error(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(Brackets(sender) + msg);
        }
        
        /// <summary>
        /// Formats and logs the given message to the console using the sender as an identifier.
        /// When the messegeLevel is equal or higher than the levelSetting.
        /// </summary>
        /// <param name="sender">reference to the sender type</param>
        /// <param name="msg">the message</param>
        /// <param name="messageLevel">LogLevel of this message.</param>
        /// <param name="levelSetting">LogLevel of this script</param>
        public static void Log(Type sender, string msg, Level messageLevel, Level levelSetting)
        {
            if (messageLevel < levelSetting) return;

            var message = TypeIdentifier(sender) + msg;

            Out(message, messageLevel);
        }

        /// <summary>
        /// Formats and logs the given message to the console using the sender as an identifier.
        /// When the messegeLevel is equal or higher than the levelSetting.
        /// </summary>
        /// <param name="sender">reference to the sender</param>
        /// <param name="msg">the message</param>
        /// <param name="messageLevel">LogLevel of this message.</param>
        /// <param name="levelSetting">LogLevel of this script</param>
        public static void Log(object sender, string msg, Level messageLevel, Level levelSetting)
        {
            if (messageLevel < levelSetting) return;

            var message = TypeIdentifier(sender) + msg;

            Out(message, messageLevel);
        }
        
        /// <summary>
        /// Formats and logs the given message to the console using the sender as an identifier.
        /// When the messegeLevel is equal or higher than the levelSetting.
        /// </summary>
        /// <param name="sender">reference to the sender</param>
        /// <param name="msg">the message</param>
        /// <param name="messageLevel">LogLevel of this message.</param>
        /// <param name="levelSetting">LogLevel of this script</param>
        public static void Log(Object sender, string msg, Level messageLevel, Level levelSetting)
        {
            if (messageLevel < levelSetting) return;

            var message = TypeIdentifier(sender) + Apostrophe(sender.name) + msg;

            Out(message, messageLevel, sender);
        }
        
        /// <summary>
        /// Formats and logs the given message to the console using the sender as an identifier.
        /// When the messegeLevel is equal or higher than the levelSetting.
        /// </summary>
        /// <param name="sender">reference to the sender</param>
        /// <param name="msg">the message</param>
        /// <param name="messageLevel">LogLevel of this message.</param>
        /// <param name="levelSetting">LogLevel of this script</param>
        public static void Log(Component sender, string msg, Level messageLevel, Level levelSetting)
        {
            if (messageLevel < levelSetting) return;

            var message = TypeIdentifier(sender) + Apostrophe(sender.gameObject.name) + msg;

            Out(message, messageLevel, sender);
        }

        /// <summary>
        /// Prints the given message based on the given message level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageLevel"></param>
        private static void Out(string message, Level messageLevel)
        {
            switch (messageLevel)
            {
                case Level.Verbose:
                case Level.Info:
                    Debug.Log(message);
                    break;
                case Level.Warning:
                    Debug.LogWarning(message);
                    break;
                case Level.Error:
                    Debug.LogWarning(message);
                    break;
            }
        }
        
        /// <summary>
        /// Prints the given message based on the given message level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageLevel"></param>
        /// <param name="sender"></param>
        private static void Out(string message, Level messageLevel, Object sender)
        {
            switch (messageLevel)
            {
                case Level.Verbose:
                case Level.Info:
                    Debug.Log(message, sender);
                    break;
                case Level.Warning:
                    Debug.LogWarning(message, sender);
                    break;
                case Level.Error:
                    Debug.LogWarning(message, sender);
                    break;
            }
        }
        
        #region Formating

        private static string TypeIdentifier(Type sender)
        {
            return Brackets(sender.Name);
        }
        
        private static string TypeIdentifier(object sender)
        {
            return Brackets(sender.GetType().Name);
        }
        
        private static string Apostrophe(string value)
        {
            return "<i>'" + value + "'</i> ";
        }
        
        private static string Asterix(string value)
        {
            return "*" + value + "* ";
        }

        private static string Brackets(string value)
        {
            return "<b>[" + value + "]</b> ";
        }
        
        #endregion

        public enum Level
        {
            Verbose = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }
    }
}