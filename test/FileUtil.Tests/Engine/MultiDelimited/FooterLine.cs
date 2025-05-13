using parsley;

namespace FileUtil.Tests.Engine.MultiDelimited
{
    public class FooterLine : PrefixedFileLine
    {
        public override string Prefix => "F";

        [Column(0)]
        public LineType Type { get; set; }

        [Column(1)]
        public int TotalRecords { get; set; }
    }
}