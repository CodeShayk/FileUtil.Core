using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ninja.FileUtil.Configuration;

namespace Ninja.FileUtil.Provider.Impl
{
    internal class DefaulProvider : IFileProvider
    {
        private readonly IProviderSettings settings;
        private readonly IFileHelper fileHelper;

        public DefaulProvider(IProviderSettings settings, IFileHelper fileHelper)
        {
            this.settings = settings;
            this.fileHelper = fileHelper;
        }

        public FileMeta[] GetFiles()
        {
            var files = new List<FileMeta>();

            var paths = fileHelper.GetPathLists(settings.FolderPath, settings.FileNameFormat);

            if (!paths.Any()) return files.ToArray();

            foreach (var path in paths)
            {
                if (!fileHelper.FileExists(path))
                    continue;

                var lines = fileHelper.ReadToLines(path);

                var fileInfo = new FileInfo(path);

                files.Add(new FileMeta
                {
                    FilePath = path,
                    FileName = fileInfo.Name,
                    FileSize = fileInfo.Length,
                    Lines = lines
                });

                if (settings.ArchiveUponRead)
                {
                    var archivePath = Path.Combine(settings.FolderPath, settings.ArchiveFolder);

                    fileHelper.EnsureFolderExist(archivePath);

                    var archiveLocation = Path.Combine(archivePath, fileInfo.Name);

                    var archiveFileInfo = new FileInfo(archiveLocation);

                    if (fileHelper.TryDeleteFile(archiveFileInfo) && fileHelper.TryMoveFile(fileInfo, archiveLocation))
                        fileHelper.TryDeleteFile(fileInfo);
                }
                else
                    fileHelper.TryDeleteFile(fileInfo);
            }

            return files.ToArray();
        }
    }
}