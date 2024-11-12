namespace FileUtil.Configuration
{
    public interface IConfigSettings
    {
        IParserSettings ParserSettings { get; set; }
        IProviderSettings ProviderSettings { get; set; }
    }
}