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

        private static string Identifier(object sender)
        {
            return Brackets(sender.GetType().Name);
        }

        private static string Brackets(string value)
        {
            return "[" + value + "] ";
        }
    }
}