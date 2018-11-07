using System.Collections.Generic;

namespace Ninja.FileUtil.Tests.Parser
{
    public class TestLine : IFileLine
    {
        public TestLine()
        {
            Errors = new List<string>();
        }

        public int Index { get; set; }
        public LineType Type { get; set; }
        public IList<string> Errors { get; set; }

        [Column(0)]
        public string Name { get; set; }
        [Column(1)]
        public bool IsMember { get; set; }
    }

}