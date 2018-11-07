using Moq;
using Ninja.FileUtil.Configuration;
using Ninja.FileUtil.Parser.Impl;
using NUnit.Framework;

namespace Ninja.FileUtil.Tests.Parser
{
     [TestFixture]
    class LineParserFixture
    {
         private Mock<IParserSettings> configuration;
         private LineParser parser;

         [SetUp]
         public void Setup()
         {
             configuration = new Mock<IParserSettings>();
             configuration.Setup(x => x.Delimiter).Returns('|');
             parser = new LineParser(configuration.Object);
         }

         [Test]
         public void TestParseForNullInputShouldReturnEmptyArray()
         {
             Assert.IsEmpty(parser.ParseWithNoLineType<TestLine>(null));
             Assert.IsEmpty(parser.ParseWithLineType<TestLine>(null, LineType.Data));
         }

         [Test]
         public void TestParseWithLineHeaderInputShouldReturnCorrectlyParsedArray()
         {
             var lines = new []
             {
                 "D|Bob Marley|True",
                 "D|John Walsh|False"
             };
             var prsed = parser.ParseWithLineType<TestLine>(lines, LineType.Data);
             
             Assert.That(prsed.Length, Is.EqualTo(2));

             Assert.That(prsed[0].Name, Is.EqualTo("Bob Marley"));
             Assert.That(prsed[0].IsMember, Is.EqualTo(true));
             Assert.That(prsed[0].Type, Is.EqualTo(LineType.Data));
             Assert.IsEmpty(prsed[0].Errors);

             Assert.That(prsed[1].Name, Is.EqualTo("John Walsh"));
             Assert.That(prsed[1].IsMember, Is.EqualTo(false));
             Assert.That(prsed[1].Type, Is.EqualTo(LineType.Data));
             Assert.IsEmpty(prsed[1].Errors);
         }

         [Test]
         public void TestParseWithNoLineHeaderInputShouldReturnCorrectlyParsedArray()
         {
             var lines = new[]
             {
                 "Bob Marley|True",
                 "John Walsh|False"
             };
             var prsed = parser.ParseWithNoLineType<TestLine>(lines);

             Assert.That(prsed.Length, Is.EqualTo(2));

             Assert.That(prsed[0].Name, Is.EqualTo("Bob Marley"));
             Assert.That(prsed[0].IsMember, Is.EqualTo(true));
             Assert.That(prsed[0].Type, Is.EqualTo(LineType.Data));
             Assert.IsEmpty(prsed[0].Errors);

             Assert.That(prsed[1].Name, Is.EqualTo("John Walsh"));
             Assert.That(prsed[1].IsMember, Is.EqualTo(false));
             Assert.That(prsed[1].Type, Is.EqualTo(LineType.Data));
             Assert.IsEmpty(prsed[1].Errors);
         }

         [TestCase("hbtrb", true)]
         [TestCase("hbtrb|ej ef|fer|", true)]
         [TestCase("H|hbtrb", false)]
         [TestCase("H|hbtrb|ej ef|fer|rc |", true)]
         public void TestParseForInvalidInputShouldReturnError(string line, bool hasLineType)
         {
             if (!hasLineType) parser = new LineParser(new TestFullConfig('|'));

             var result = hasLineType
                 ? parser.ParseWithNoLineType<TestLine>(new[] {line})
                 : parser.ParseWithLineType<TestLine>(new[] {line}, LineType.Header);

             Assert.IsNotEmpty(result[0].Errors);
         }

         [Test]
        public void TestParseForInvalidFileLineWithNoColumnAttributesShouldReturnError()
        {

            var result = parser.ParseWithLineType<InvalidTestLine>(new[] { "edndx|medmd" }, LineType.Data);

            Assert.IsNotEmpty(result[0].Errors);

            result = parser.ParseWithNoLineType<InvalidTestLine>(new[] { "edndx|medmd" });

            Assert.IsNotEmpty(result[0].Errors);
        }
    }

    public class TestSimpleConfig : IDelimiter
    {
        public TestSimpleConfig(char delimeter)
        {
            Delimiter = delimeter;
        }

        public char Delimiter { get; set; }
    }

    public class TestFullConfig : IParserSettings
    {
        public TestFullConfig(char delimeter)
        {
            Delimiter = delimeter;
            Header = "H";
            Footer = "F";
            Data = "D";
        }

        public char Delimiter { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Data { get; set; }
    }
    
}
