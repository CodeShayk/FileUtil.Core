namespace Ninja.FileUtil.Configuration
{
    public interface IProviderSettings
    {
        string FolderPath { get; set; }
        string FileNameFormat { get; set; }
        bool ArchiveUponRead { get; set; }
        string ArchiveFolder { get; set; }
    }
}