using Ninja.FileUtil.Configuration;

namespace Ninja.FileUtil.Tests.Configuration
{
    public class LineHeaders : ILineHeaders
    {
        public LineHeaders(string header = "H", string footer = "F", string data = "D")
        {
            Header = header;
            Footer = footer;
            Data = data;
        }

        public string Header { get; set; }
        public string Footer { get; set; }
        public string Data { get; set; }
    }
}