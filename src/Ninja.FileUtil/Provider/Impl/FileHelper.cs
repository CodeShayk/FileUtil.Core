using System.Collections.Generic;
using System.IO;

namespace Ninja.FileUtil.Provider.Impl
{
    internal class FileHelper : IFileHelper
    {
        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool TryMoveFile(FileInfo fileInfo, string destinationFile)
        {
            try
            {
                fileInfo.MoveTo(destinationFile);
                return true;
            }
            catch (IOException) 
            {
                return false;
            }
        }

        public bool TryDeleteFile(FileInfo fileInfo)
        {
            try
            {
                if (fileInfo.Exists)
                    fileInfo.Delete();

                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
        public void EnsureFolderExist(string folderFullName)
        {
            if (!Directory.Exists(folderFullName))
                Directory.CreateDirectory(folderFullName);
        }

        public string[] GetPathLists(string folderPath, string fileFormat)
        {
            var filePaths = !string.IsNullOrWhiteSpace(fileFormat)
                ? Directory.GetFiles(folderPath, fileFormat, SearchOption.TopDirectoryOnly)
                : Directory.GetFiles(folderPath);

            return filePaths;
        }

        public string[] ReadToLines(string path)
        {
            var lines = new List<string>();
            using (var sr = new StreamReader(File.Open(path, FileMode.Open)))
            {
                var line = sr.ReadLine();
                if (line != null)
                    lines.Add(line);
            }

            return lines.ToArray();
        }
    }
}