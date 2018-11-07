using System.Collections.Generic;

namespace Ninja.FileUtil.Tests.Parser
{
    public class InvalidTestLine : IFileLine
    {
        public int Index { get; set; }
        public LineType Type { get; set; }
        public IList<string> Errors { get; set; }
    }
}