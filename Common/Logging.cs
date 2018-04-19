using UnityEngine;

namespace UE.Common
{
    public static class Logging
    {
        public static void Log(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(Identifier(sender) + msg);
        }

        public static void Log(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(Brackets(sender) + msg);
        }

        public static void Warning(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(Identifier(sender) + msg);
        }

        public static void Warning(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(Brackets(sender) + msg);
        }

        public static void Error(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(Identifier(sender) + msg);
        }

        public static void Error(string sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(Brackets(sender) + msg);
        }

        /// <summary>
        /// Formats and logs the given message to the unity log.
        /// </summary>
        /// <param name="sender">reference to the sender</param>
        /// <param name="msg">the message</param>
        /// <param name="messageLevel">LogLevel of this message.</param>
        /// <param name="levelSetting">LogLevel of this script</param>
        public static void Log(object sender, string msg, Level messageLevel, Level levelSetting)
        {
            if (messageLevel < levelSetting) return;

            var message = Identifier(sender) + msg;

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

        private static string Identifier(object sender)
        {
            return Brackets(sender.GetType().Name);
        }

        private static string Brackets(string value)
        {
            return "[" + value + "] ";
        }

        public enum Level
        {
            Verbose = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }
    }
}