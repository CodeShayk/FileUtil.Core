namespace Ninja.FileUtil.Configuration
{
    public interface IParserSettings: IDelimiter
    {
        string Header { get; set; }
        string Footer { get; set; }
        string Data { get; set; }
    }
}