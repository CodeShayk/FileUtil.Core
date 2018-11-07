using System.IO;

namespace Ninja.FileUtil.Provider
{
    public interface IFileHelper
    {
        bool FileExists(string path);
        bool TryMoveFile(FileInfo fileInfo, string destinationFile);
        bool TryDeleteFile(FileInfo fileInfo);
        void EnsureFolderExist(string folderFullName);
        string[] GetPathLists(string folderPath, string fileFormat);
        string[] ReadToLines(string path);
    }
}