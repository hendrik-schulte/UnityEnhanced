using UnityEngine;

namespace UE.Common
{
    public static class Logging
    {
        public static void Log(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.Log(Identifier(sender) + msg);
        }

        public static void Warning(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogWarning(Identifier(sender) + msg);
        }

        public static void Error(object sender, string msg, bool debugLog = true)
        {
            if (debugLog) Debug.LogError(Identifier(sender) + msg);
        }

        private static string Identifier(object sender)
        {
            return "[" + sender.GetType().Name + "] ";
        }
       
//        public enum LoggingLevel
//        {
//            
//        }
    }
}