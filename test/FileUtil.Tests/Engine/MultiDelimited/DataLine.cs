using parsley;

namespace FileUtil.Tests.Engine.MultiDelimited
{
    public class DataLine : PrefixedFileLine
    {
        public override string Prefix => "D";

        [Column(0)]
        public LineType Type { get; set; }

        [Column(1)]
        public string Employee { get; set; }

        [Column(2)]
        public string Reference { get; set; }

        [Column(3)]
        public bool InService { get; set; }
    }
}