namespace Ninja.FileUtil.Parser
{
    internal interface ILineParser
    {
        T[] Parse<T>(string[] lines) where T : IFileLine, new();
        T[] Parse<T>(string[] lines, LineType type) where T : IFileLine, new();
    }
}