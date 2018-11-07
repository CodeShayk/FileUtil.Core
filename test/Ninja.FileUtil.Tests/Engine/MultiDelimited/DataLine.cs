namespace Ninja.FileUtil.Tests.Engine.MultiDelimited
{
    public class DataLine : FileLine
    {
        [Column(0)]
        public string  Employee { get; set; }
        [Column(1)]
        public string Reference { get; set; }
        [Column(2)]
        public bool InService { get; set; }
    }
}