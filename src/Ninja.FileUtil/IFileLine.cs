using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ninja.FileUtil.Tests")]
namespace Ninja.FileUtil
{
    internal interface IFileLine
    {
        int Index { get; set; }
        LineType Type { get; set; }
        IList<string> Errors { get; set; }
    }
}