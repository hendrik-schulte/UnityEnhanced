using UnityEngine;

namespace UE.Common
{
    /// <summary>
    /// This clears all file streams opened by the FileLogger class when the application quits.
    /// </summary>
    public class FileStreamCloser : MonoBehaviour
    {
        [SerializeField]
        private bool debugLog;
        
        private void OnDestroy()
        {
            Logging.Log(this, "Closing all StreamWriters", debugLog);
            
            FileLogger.Close();
        }
    }
}