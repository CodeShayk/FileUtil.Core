//using System;
//using System.Configuration;
//using System.IO;
//using System.Reflection;
//using Ninja.FileUtil.Configuration;
//using NUnit.Framework;

//namespace Ninja.FileUtil.Tests.Configuration
//{

//    [TestFixture]
//    public class SettingsFixture
//    {
//        private string completeConfigFilePath;
//        private string minimalConfigFileName;
       
//        private const char Delimeter =',';
//        private const string Header = "H1";
//        private const string Data = "D1";
//        private const string Footer = "F1";
       
//        private const string FolderPath = "http://localhost:49320/Aegon/Target";
//        private const string FileNameFormat = "DailyFeed_*.txt";
//        private const string ArchiveUponRead = "true";
//        private const string ArchiveFolder = "Archive";
       

//        [SetUp]
//        public void Setup()
//        {
//           completeConfigFilePath = CreateConfigFile("complete.exe.config", 
//            @"<configuration>
//                <configSections>
//                    <sectionGroup name=""FileUtil"">
//                        <section name=""Settings"" type=""Ninja.FileUtil.Configuration.Settings, Ninja.FileUtil"" />
//                    </sectionGroup>
//                </configSections>
//                <FileUtil>
//                    <Settings>
//                        <Parser  delimiter=""" + Delimeter +
//                                    @""" header=""" + Header +
//                                    @""" data=""" + Data +
//                                    @""" footer=""" + Footer +
//                                    @"""/>                                                                             

//                        <Provider folderPath=""" + FolderPath +
//                                    @""" fileNameFormat=""" + FileNameFormat +
//                                    @""" archiveUponRead=""" + ArchiveUponRead +
//                                    @""" archiveFolder=""" + ArchiveFolder + @"""/>                                     
//                                        </Settings>
//                </FileUtil>
//                            </configuration>");

//           minimalConfigFileName = CreateConfigFile("minimal.exe.config", @"<configuration>
//                <configSections>
//                    <sectionGroup name=""FileUtil"">
//                        <section name=""Settings"" type=""Ninja.FileUtil.Configuration.Settings, Ninja.FileUtil"" />
//                    </sectionGroup>
//                </configSections>
//                <FileUtil>
//                    <Settings>
//                        <Provider folderPath=""" + FolderPath + @"""/>                                     
//                    </Settings>
//                </FileUtil>
//                            </configuration>");

//        }

//        private string CreateConfigFile(string fileName, string configuration)
//        {
//            var configFilePath = Path.Combine(AssemblyDirectory, fileName);

//            if (File.Exists(configFilePath))
//                File.Delete(configFilePath);

//            using (var sw = File.CreateText(configFilePath))
//            {
//                sw.WriteLine(configuration);
//            }

//            return configFilePath;
//        }


//        [Test]
//        public void ValidateCompleteConfigSettings()
//        {

//            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = completeConfigFilePath };

//            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
//            var section = (Settings)config.GetSection(Settings.SectionXPath);

//            Assert.That(section.ProviderSettings.FolderPath.Equals(FolderPath));
//            Assert.That(section.ProviderSettings.FileNameFormat.Equals(FileNameFormat));
//            Assert.That(section.ProviderSettings.ArchiveUponRead, Is.EqualTo(true));
//            Assert.That(section.ProviderSettings.ArchiveFolder.Equals(ArchiveFolder));

//            Assert.That(section.ParserSettings.Delimiter.Equals(Delimeter));
//            Assert.That(section.ParserSettings.Header.Equals(Header));
//            Assert.That(section.ParserSettings.Data.Equals(Data));
//            Assert.That(section.ParserSettings.Footer.Equals(Footer));
           
//        }

//        [Test]
//        public void ValidateMinimalDefaultConfigSettings()
//        {

//            var configFileMap = new ExeConfigurationFileMap { ExeConfigFilename = minimalConfigFileName };

//            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
//            var section = (Settings)config.GetSection(Settings.SectionXPath);

//            Assert.That(section.ProviderSettings.FolderPath.Equals(FolderPath));
//            Assert.IsTrue(string.IsNullOrEmpty(section.ProviderSettings.FileNameFormat));
//            Assert.That(section.ProviderSettings.ArchiveUponRead, Is.EqualTo(Ninja.FileUtil.Configuration.ProviderSettings.ArchiveUponReadDefault));
//            Assert.That(section.ProviderSettings.ArchiveFolder.Equals(Ninja.FileUtil.Configuration.ProviderSettings.ArchiveFolderDefault));

//            Assert.That(section.ParserSettings.Delimiter.Equals(ParserSettings.DelimiterDefault));
//            Assert.That(section.ParserSettings.Header.Equals(ParserSettings.HeaderDefault));
//            Assert.That(section.ParserSettings.Data.Equals(ParserSettings.DataDefault));
//            Assert.That(section.ParserSettings.Footer.Equals(ParserSettings.FooterDefault));

//        }

       
//        static public string AssemblyDirectory
//        {
//            get
//            {
//                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
//                var uri = new UriBuilder(codeBase);
//                var path = Uri.UnescapeDataString(uri.Path);
//                return Path.GetDirectoryName(path);
//            }
//        }

//    }
//}
