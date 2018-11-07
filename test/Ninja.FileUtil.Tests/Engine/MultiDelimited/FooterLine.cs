namespace Ninja.FileUtil.Tests.Engine.MultiDelimited
{
    public class FooterLine : FileLine
    {
        [Column(0)]
        public int TotalRecords { get; set; }
    }
}