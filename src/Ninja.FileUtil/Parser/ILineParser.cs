namespace Ninja.FileUtil.Parser
{
    internal interface ILineParser
    {
        T[] ParseWithNoLineType<T>(string[] lines) where T : IFileLine, new();
        T[] ParseWithLineType<T>(string[] lines, LineType type) where T : IFileLine, new();
    }

}