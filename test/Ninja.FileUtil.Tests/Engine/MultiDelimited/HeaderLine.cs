using System;

namespace Ninja.FileUtil.Tests.Engine.MultiDelimited
{
    public class HeaderLine : FileLine
    {
        [Column(0)]
        public DateTime Date { get; set; }
        [Column(1)]
        public string Name { get; set; }
    }
}