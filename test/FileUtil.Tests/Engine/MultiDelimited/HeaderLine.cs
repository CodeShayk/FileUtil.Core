using parsley;

namespace FileUtil.Tests.Engine.MultiDelimited
{
    public class HeaderLine : PrefixedFileLine
    {
        public override string Prefix => "H";

        [Column(0)]
        public LineType Type { get; set; }

        [Column(1)]
        public DateTime Date { get; set; }

        [Column(2)]
        public string Name { get; set; }
    }
}