using System.Collections.Generic;
using parsley;

namespace FileUtil
{
    public abstract class PrefixedFileLine : IFileLine
    {
        protected PrefixedFileLine()
        {
            Errors = new List<string>();
        }

        public abstract string Prefix { get; }

        public int Index { get; set; }
        public IList<string> Errors { get; set; }
    }
}