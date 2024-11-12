namespace FileUtil.Configuration
{
    public interface ILineHeaders
    {
        string Header { get; set; }
        string Footer { get; set; }
        string Data { get; set; }
    }
}