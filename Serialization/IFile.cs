namespace UE.Serialization
{
    /// <summary>
    /// If an object implementing this interface is deserialized, its file name and path is passed
    /// to it by the SetPath function.
    /// </summary>
    public interface IFile
    {
        /// <summary>
        /// On deserialization after reading a file from disc, this
        /// functions sets the file name and path of the original file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        void SetPath(string path, string name);
    }
}