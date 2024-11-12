using System.Collections.Generic;

namespace FileUtil
{
    internal interface IFileLine
    {
        int Index { get; set; }
        LineType Type { get; set; }
        IList<string> Errors { get; set; }
    }
}