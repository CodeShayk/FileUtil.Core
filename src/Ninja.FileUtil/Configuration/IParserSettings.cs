namespace Ninja.FileUtil.Configuration
{
    public interface IParserSettings
    {
        IDelimiter Delimiter { get; set; }
        ILineHeaders LineHeaders { get; set; }
    }
}