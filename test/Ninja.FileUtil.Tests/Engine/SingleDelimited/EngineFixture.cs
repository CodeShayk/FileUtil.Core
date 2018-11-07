using Moq;
using Ninja.FileUtil.Configuration;
using Ninja.FileUtil.Provider;
using NUnit.Framework;

namespace Ninja.FileUtil.Tests.Engine.SingleDelimited
{
    [TestFixture]
    public class EngineFixture
    {
        private Mock<IFileProvider> provider;
        private Mock<IParserSettings> configuration;
        private Engine<SingleLine> engine;

        [SetUp]
        public void Setup()
        {
            provider = new Mock<IFileProvider>();
            configuration = new Mock<IParserSettings>();
            configuration.Setup(x => x.Delimiter).Returns('|');
            engine = new Engine<SingleLine>(configuration.Object, provider.Object);
        }

        [Test]
        public void TestGetFilesForNoFileFromProviderShouldReturnEmptyCollection()
        {
           Assert.IsEmpty(engine.GetFiles());
        }

        [Test]
        public void TestGetFilesForFileReceivedFromProviderShouldReturnEmptyCollection()
        {
            var fileMeta = new FileMeta
            {
                FileName = "name",
                FilePath = "path",
                FileSize = 1234,
                Lines = new[] {"Jack Marias|false", "Samuel Dias|true"}
            };

            provider.Setup(x => x.GetFiles()).Returns(new[] { fileMeta });

            var parsedfiles = engine.GetFiles();

            Assert.IsNotEmpty(parsedfiles);
            Assert.That(parsedfiles[0].FileMeta.FileName, Is.EqualTo(fileMeta.FileName));
            Assert.That(parsedfiles[0].FileMeta.FilePath, Is.EqualTo(fileMeta.FilePath));
            Assert.That(parsedfiles[0].FileMeta.FileSize, Is.EqualTo(fileMeta.FileSize));
            Assert.That(parsedfiles[0].FileMeta.Lines, Is.EqualTo(fileMeta.Lines));


            Assert.IsAssignableFrom<SingleLine>(parsedfiles[0].Data[0]);
            Assert.IsAssignableFrom<SingleLine>(parsedfiles[0].Data[1]);

            Assert.That(parsedfiles[0].Data[0].Index, Is.EqualTo(0));
            Assert.That(parsedfiles[0].Data[0].Type, Is.EqualTo(LineType.Data));
            Assert.IsEmpty(parsedfiles[0].Data[0].Errors);

            Assert.That(parsedfiles[0].Data[0].Name, Is.EqualTo("Jack Marias"));
            Assert.That(parsedfiles[0].Data[0].IsMember, Is.EqualTo(false));


            Assert.That(parsedfiles[0].Data[1].Index, Is.EqualTo(1));
            Assert.That(parsedfiles[0].Data[1].Type, Is.EqualTo(LineType.Data));
            Assert.IsEmpty(parsedfiles[0].Data[0].Errors);

            Assert.That(parsedfiles[0].Data[1].Name, Is.EqualTo("Samuel Dias"));
            Assert.That(parsedfiles[0].Data[1].IsMember, Is.EqualTo(true));
           
        }
    }
}
