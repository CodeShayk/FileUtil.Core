using FileUtil.Configuration;
using FileUtil.Parser.Impl;
using Moq;
using NUnit.Framework;

namespace FileUtil.Tests.Parser
{
    [TestFixture]
    internal class LineParserFixture
    {
        private Mock<IParserSettings> configuration;
        private LineParser parser;

        [SetUp]
        public void Setup()
        {
            configuration = new Mock<IParserSettings>();

            configuration.Setup(x => x.Delimiter.Value).Returns('|');
            configuration.Setup(x => x.LineHeaders.Header).Returns("H");
            configuration.Setup(x => x.LineHeaders.Data).Returns("D");
            configuration.Setup(x => x.LineHeaders.Footer).Returns("F");
            parser = new LineParser(configuration.Object);
        }

        [Test]
        public void TestParseForNullInputShouldReturnEmptyArray()
        {
            Assert.That(parser.Parse<TestLine>(null), Is.Empty);
            Assert.That(parser.Parse<TestLine>(null, LineType.Data), Is.Empty);
        }

        [Test]
        public void TestParseWithLineHeaderInputShouldReturnCorrectlyParsedArray()
        {
            var lines = new[]
            {
                 "D|Bob Marley|True",
                 "D|John Walsh|False"
             };

            var parsed = parser.Parse<TestLine>(lines, LineType.Data);

            Assert.That(parsed.Length, Is.EqualTo(2));

            Assert.That(parsed[0].Name, Is.EqualTo("Bob Marley"));
            Assert.That(parsed[0].IsMember, Is.EqualTo(true));
            Assert.That(parsed[0].Type, Is.EqualTo(LineType.Data));
            Assert.That(parsed[0].Errors, Is.Empty);

            Assert.That(parsed[1].Name, Is.EqualTo("John Walsh"));
            Assert.That(parsed[1].IsMember, Is.EqualTo(false));
            Assert.That(parsed[1].Type, Is.EqualTo(LineType.Data));
            Assert.That(parsed[1].Errors, Is.Empty);
        }

        [Test]
        public void TestParseWithNoLineHeaderInputShouldReturnCorrectlyParsedArray()
        {
            var lines = new[]
            {
                 "Bob Marley|True",
                 "John Walsh|False"
             };
            var prsed = parser.Parse<TestLine>(lines);

            Assert.That(prsed.Length, Is.EqualTo(2));

            Assert.That(prsed[0].Name, Is.EqualTo("Bob Marley"));
            Assert.That(prsed[0].IsMember, Is.EqualTo(true));
            Assert.That(prsed[0].Type, Is.EqualTo(LineType.Data));
            Assert.That(prsed[0].Errors, Is.Empty);

            Assert.That(prsed[1].Name, Is.EqualTo("John Walsh"));
            Assert.That(prsed[1].IsMember, Is.EqualTo(false));
            Assert.That(prsed[1].Type, Is.EqualTo(LineType.Data));
            Assert.That(prsed[1].Errors, Is.Empty);
        }

        [TestCase("hbtrb", true)]
        [TestCase("hbtrb|ej ef|fer|", true)]
        [TestCase("H|hbtrb", false)]
        [TestCase("H|hbtrb|ej ef|fer|rc |", true)]
        public void TestParseForInvalidInputShouldReturnError(string line, bool hasLineType)
        {
            if (!hasLineType)
                parser = new LineParser(configuration.Object);

            var result = hasLineType
                ? parser.Parse<TestLine>(new[] { line })
                : parser.Parse<TestLine>(new[] { line }, LineType.Header);

            Assert.That(result[0].Errors, Is.Not.Empty);
        }

        [Test]
        public void TestParseForInvalidFileLineWithNoColumnAttributesShouldReturnError()
        {
            var result = parser.Parse<InvalidTestLine>(new[] { "D|edndx|medmd" }, LineType.Data);

            Assert.That(result[0].Errors, Is.Not.Empty);

            result = parser.Parse<InvalidTestLine>(new[] { "edndx|medmd" });

            Assert.That(result[0].Errors, Is.Not.Empty);
        }
    }
}