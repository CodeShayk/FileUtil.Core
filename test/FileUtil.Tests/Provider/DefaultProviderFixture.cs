using FileUtil.Configuration;
using FileUtil.Provider;
using FileUtil.Provider.Impl;
using Moq;
using NUnit.Framework;

namespace FileUtil.Tests.Provider
{
    [TestFixture]
    internal class DefaultProviderFixture
    {
        private Mock<IProviderSettings> settings;
        private Mock<IFileHelper> fileHelper;
        private DefaulProvider provider;
        private string filePath = @"TestFile.txt";

        [SetUp]
        public void Setup()
        {
            settings = new Mock<IProviderSettings>();
            fileHelper = new Mock<IFileHelper>();
            provider = new DefaulProvider(settings.Object, fileHelper.Object);
        }

        [Test]
        public void TestGetFilesForNoFilesShouldReturnEmptyFileCollection()
        {
            fileHelper.Setup(x => x.GetPathLists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Enumerable.Empty<string>().ToArray());

            Assert.That(provider.GetFiles(), Is.Empty);
        }

        [Test]
        public void TestGetFilesForFileWhichDoesNotExistsShouldReturnEmptyFileCollection()
        {
            fileHelper.Setup(x => x.GetPathLists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { "path" });

            fileHelper.Setup(x => x.FileExists("path")).Returns(false);

            Assert.That(provider.GetFiles(), Is.Empty);
        }

        [Test]
        public void TestGetFilesForFileWhichExistsAndWithNoArchiveSettingShouldReturnFileCollectionWithFileInfo()
        {
            fileHelper.Setup(x => x.GetPathLists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { filePath });

            fileHelper.Setup(x => x.FileExists(filePath)).Returns(true);
            fileHelper.Setup(x => x.ReadToLines(filePath)).Returns(new[] { "xyz, abc" });

            Assert.That(provider.GetFiles(), Is.Not.Empty);
            fileHelper.Verify(x => x.TryDeleteFile(It.IsAny<FileInfo>()), Times.Exactly(1));
            fileHelper.Verify(x => x.TryMoveFile(It.IsAny<FileInfo>(), It.IsAny<string>()), Times.Never());
            fileHelper.Verify(x => x.EnsureFolderExist(It.IsAny<string>()), Times.Never());
        }

        [Test]
        public void TestGetFilesForFileWhichExistsAndWithArchiveSettingShouldReturnFileCollectionWithFileInfo()
        {
            settings.Setup(x => x.ArchiveUponRead).Returns(true);
            settings.Setup(x => x.FolderPath).Returns("c:work");
            settings.Setup(x => x.FileNameFormat).Returns("testFile.txt");
            settings.Setup(x => x.ArchiveFolder).Returns("Archived");

            fileHelper.Setup(x => x.TryDeleteFile(It.IsAny<FileInfo>())).Returns(true);
            fileHelper.Setup(x => x.TryMoveFile(It.IsAny<FileInfo>(), It.IsAny<string>())).Returns(true);

            fileHelper.Setup(x => x.GetPathLists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[] { filePath });

            fileHelper.Setup(x => x.FileExists(filePath)).Returns(true);
            fileHelper.Setup(x => x.ReadToLines(filePath)).Returns(new[] { "xyz, abc" });

            Assert.That(provider.GetFiles(), Is.Not.Empty);
            fileHelper.Verify(x => x.TryDeleteFile(It.IsAny<FileInfo>()), Times.Exactly(2));
            fileHelper.Verify(x => x.TryMoveFile(It.IsAny<FileInfo>(), It.IsAny<string>()), Times.Exactly(1));
            fileHelper.Verify(x => x.EnsureFolderExist(It.IsAny<string>()), Times.Exactly(1));
        }
    }
}