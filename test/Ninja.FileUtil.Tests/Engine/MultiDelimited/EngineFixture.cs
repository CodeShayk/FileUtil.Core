using System;
using Moq;
using Ninja.FileUtil.Configuration;
using Ninja.FileUtil.Provider;
using NUnit.Framework;

namespace Ninja.FileUtil.Tests.Engine.MultiDelimited
{
    [TestFixture]
    public class EngineFixture
    {
        private Mock<IFileProvider> provider;
        private Mock<IParserSettings> configuration;
        private Mock<IDelimiter> delimiter;
        private FileUtil.Engine engine;

        [SetUp]
        public void Setup()
        {
            provider = new Mock<IFileProvider>();
            configuration = new Mock<IParserSettings>();
            delimiter = new Mock<IDelimiter>();
            delimiter.Setup(x => x.Value).Returns('|');
            configuration.Setup(x => x.Delimiter).Returns(delimiter.Object);
            configuration.Setup(x => x.LineHeaders.Header).Returns("H");
            configuration.Setup(x => x.LineHeaders.Data).Returns("D");
            configuration.Setup(x => x.LineHeaders.Footer).Returns("F");

            engine = new FileUtil.Engine(configuration.Object, provider.Object);
        }

        [Test]
        public void TestGetFilesForNoFileFromProviderShouldReturnEmptyCollection()
        {
            Assert.IsEmpty(engine.GetFiles<HeaderLine, DataLine, FooterLine>());
        }

        [Test]
        public void TestGetFilesForFileReceivedFromProviderShouldReturnEmptyCollection()
        {
            var date = new DateTime(2016, 10,22);
            var fileMeta = new FileMeta
            {
                FileName = "name",
                FilePath = "path",
                FileSize = 1234,
                Lines = new[] {$"H|{date.ToShortDateString()}|Employee Status", "D|John Walsh|456RT4|True", "F|1" }
            };

            provider.Setup(x => x.GetFiles()).Returns(new[] { fileMeta });

            var parsedfiles = engine.GetFiles<HeaderLine, DataLine, FooterLine>();

            Assert.IsNotEmpty(parsedfiles);
            Assert.That(parsedfiles[0].FileMeta.FileName, Is.EqualTo(fileMeta.FileName));
            Assert.That(parsedfiles[0].FileMeta.FilePath, Is.EqualTo(fileMeta.FilePath));
            Assert.That(parsedfiles[0].FileMeta.FileSize, Is.EqualTo(fileMeta.FileSize));
            Assert.That(parsedfiles[0].FileMeta.Lines, Is.EqualTo(fileMeta.Lines));

            Assert.IsAssignableFrom<HeaderLine>(parsedfiles[0].Header);

            Assert.That(parsedfiles[0].Header.Index, Is.EqualTo(0));
            Assert.That(parsedfiles[0].Header.Type, Is.EqualTo(LineType.Header));
            Assert.IsEmpty(parsedfiles[0].Header.Errors);
            Assert.That(parsedfiles[0].Header.Date, Is.EqualTo(date));
            Assert.That(parsedfiles[0].Header.Name, Is.EqualTo("Employee Status"));
          
            
            Assert.IsAssignableFrom<DataLine>(parsedfiles[0].Data[0]);

            Assert.That(parsedfiles[0].Data[0].Index, Is.EqualTo(0));
            Assert.That(parsedfiles[0].Data[0].Type, Is.EqualTo(LineType.Data));
            Assert.IsEmpty(parsedfiles[0].Data[0].Errors);

            Assert.That(parsedfiles[0].Data[0].Employee, Is.EqualTo("John Walsh"));
            Assert.That(parsedfiles[0].Data[0].Reference, Is.EqualTo("456RT4"));
            Assert.That(parsedfiles[0].Data[0].InService, Is.EqualTo(true));
           
            Assert.IsAssignableFrom<FooterLine>(parsedfiles[0].Footer);

            Assert.That(parsedfiles[0].Footer.Index, Is.EqualTo(0));
            Assert.That(parsedfiles[0].Footer.Type, Is.EqualTo(LineType.Footer));
            Assert.IsEmpty(parsedfiles[0].Footer.Errors);

            Assert.That(parsedfiles[0].Footer.TotalRecords, Is.EqualTo(1));
        }
    }
}
