namespace Ninja.FileUtil.Tests.Engine.SingleDelimited
{
    public class SingleLine : FileLine
    {
        [Column(0)]
        public string Name { get; set; }
        [Column(1)]
        public bool IsMember { get; set; }
    }
}