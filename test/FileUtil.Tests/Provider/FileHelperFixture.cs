using FileUtil.Provider.Impl;
using NUnit.Framework;

namespace FileUtil.Tests.Provider
{
    [TestFixture]
    internal class FileHelperFixture
    {
        private string filePath;

        private FileHelper fileHelper;

        [SetUp]
        public void Setup()
        {
            fileHelper = new FileHelper();
            filePath = Path.Combine(Environment.CurrentDirectory, "TestFile.txt");
            CreateFile(filePath);
        }

        [Test]
        public void TestGetFilesForFileWhichDoesNotExistsShouldReturnEmptyFileCollection()
        {
            var lines = fileHelper.ReadToLines(filePath);
            Assert.That(lines.Length, Is.EqualTo(1));
        }

        public void CreateFile(string path)
        {
            using (var sr = new StreamWriter(File.Open(path, FileMode.OpenOrCreate)))
                sr.Write("test one");
        }
    }
}