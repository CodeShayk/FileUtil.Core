using System.Linq;
using Ninja.FileUtil.Configuration;
using Ninja.FileUtil.Parser;
using Ninja.FileUtil.Parser.Impl;
using Ninja.FileUtil.Provider;
using Ninja.FileUtil.Provider.Impl;

namespace Ninja.FileUtil
{
    public class Engine<T> where T : FileLine, new()
    {
        private readonly IFileProvider fileProvider;
        private readonly ILineParser lineParser;

        internal Engine(IFileProvider fileProvider, ILineParser lineParser)
        {
            this.fileProvider = fileProvider;
            this.lineParser = lineParser;
        }
        /// <summary>
        /// Create Single line type Engine instance with default parser.
        /// </summary>
        /// <param name="parserSettings">Parser setting instance.</param>
        /// <param name="fileProvider">File provider instance.</param>
        public Engine(IParserSettings parserSettings, IFileProvider fileProvider)
            : this(fileProvider, new LineParser(parserSettings))
        {

        }
        /// <summary>
        /// Create Multi line type Engine instance with default parser and default file provider.
        /// </summary>
        /// <param name="settings">Configuration settings for default file provider and default parser</param>
        public Engine(IConfigSettings settings)
            : this(settings.ParserSettings, new DefaulProvider(settings.ProviderSettings, new FileHelper()))
        {

        }

        /// <summary>
        /// Get all single fixed format lines from a text file parsed into a strongly typed array
        /// Default delimiter is '|'
        /// Example File -
        /// "John Walsh|456RT4|True|Male"
        /// "Simone Walsh|456RT5|True|Female"
        /// </summary>
        /// <typeparam name="T">Typed Line Class</typeparam>
        /// <returns>
        /// Collection of Files each parsed with typed class arrays
        /// </returns>
        public File<T>[] GetFiles()
        {
            var files = fileProvider.GetFiles();
            return files.Select(file => new File<T>
            {
                FileMeta = new FileMeta
                {
                    FileName = file.FileName,
                    FilePath = file.FilePath,
                    FileSize = file.FileSize,
                    Lines = file.Lines,
                },

                Data = lineParser.ParseWithNoLineType<T>(file.Lines)
            })
            .ToArray();
        }
    }

    public class Engine<TH, TD, TF> where TH : FileLine, new()
                                    where TD : FileLine, new()
                                    where TF : FileLine, new()
    {
        private readonly IFileProvider fileProvider;
        private readonly ILineParser lineParser;
        private readonly IParserSettings parserSettings;

        internal Engine(IFileProvider fileProvider, ILineParser lineParser)
        {
            this.fileProvider = fileProvider;
            this.lineParser = lineParser;
        }
        /// <summary>
        /// Create Multi line type Engine instance with default parser.
        /// </summary>
        /// <param name="parserSettings">Parser setting instance.</param>
        /// <param name="fileProvider">File provider instance.</param>
        public Engine(IParserSettings parserSettings, IFileProvider fileProvider)
            : this(fileProvider, new LineParser(parserSettings))
        {
            this.parserSettings  = parserSettings;
        }

        /// <summary>
        /// Create Multi line type Engine instance with default parser and default file provider.
        /// </summary>
        /// <param name="settings">Configuration settings for default file provider and default parser</param>
        public Engine(IConfigSettings settings)
            : this(settings.ParserSettings, new DefaulProvider(settings.ProviderSettings, new FileHelper()))
        {

        }

        /// <summary>
        /// Get all multi-format lines from a text file parsed into header, data and footer 
        /// typed arrays respectively.
        /// Header line starts with H, data line starts with D and 
        /// footer line starts with F by defaults 
        /// Example File - 
        /// "H|22-10-2016|Employee Status"
        /// "D|John Walsh|456RT4|True"
        /// "D|Mark Walsh|456RT5|True"
        /// "F|2"
        /// </summary>
        /// <typeparam name="TH">Typed Header Line Class</typeparam>
        /// <typeparam name="TD">Typed Data Line Class</typeparam>
        /// <typeparam name="TF">Typed Footer Line Class</typeparam>
        /// <returns>
        /// Collection of Files each parsed with header, footer and data typed arrays
        /// </returns>
        public File<TH, TD, TF>[] GetFiles()
        {
            var files = fileProvider.GetFiles();

            return files.Select(file =>
            {
                var parsed = new File<TH, TD, TF>
                {
                    FileMeta = new FileMeta
                    {
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                        FileSize = file.FileSize,
                        Lines = file.Lines,
                    },

                    Headers = lineParser.ParseWithLineType<TH>(file.Lines.Where(x => x.StartsWith(parserSettings.Header)).ToArray(), LineType.Header),
                    Footers = lineParser.ParseWithLineType<TF>(file.Lines.Where(x => x.StartsWith(parserSettings.Footer)).ToArray(), LineType.Footer),
                    Data = lineParser.ParseWithLineType<TD>(file.Lines.Where(x => x.StartsWith(parserSettings.Data)).ToArray(), LineType.Data)
                };

                return parsed;

            }).ToArray();
        }
    }
}
