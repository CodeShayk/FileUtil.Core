using Ninja.FileUtil.Configuration;

namespace Ninja.FileUtil.Tests.Configuration
{
    public class TestFullConfig : IParserSettings
    {
        public TestFullConfig(char delimeter)
        {
            Delimiter = new Delimiter(delimeter);
            LineHeaders = new LineHeaders();
        }

        public IDelimiter Delimiter { get; set; }
        public ILineHeaders LineHeaders { get; set; }
    }
}