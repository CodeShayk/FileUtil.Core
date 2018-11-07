using System.Collections.Generic;

namespace Ninja.FileUtil
{
    public abstract class FileLine : IFileLine
    {
        protected FileLine()
        {
            Errors = new List<string>();
        }

        public int Index { get; set; }
        public LineType Type { get; set; }
        public IList<string> Errors { get; set; }
    }
}