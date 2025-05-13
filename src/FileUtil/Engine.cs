using System.Collections.Generic;
using System.Linq;
using FileUtil.Configuration;
using FileUtil.Provider;
using FileUtil.Provider.Impl;
using parsley;

namespace FileUtil
{
    public class Engine
    {
        private readonly IFileProvider fileProvider;
        private readonly IParser parser;

        internal Engine(IParser parser, IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
            this.parser = parser;
        }

        /// <summary>
        /// Create Engine instance with default parser and file provider.
        /// </summary>
        /// <remarks>
        /// You need to provide the parser and file provider settings.
        /// </remarks>
        /// <param name="settings">Configuration settings for default file provider and parser</param>
        public Engine(IConfigSettings settings)
            : this(new Parser((settings?.ParserSettings?.Delimiter?.Value).GetValueOrDefault(',')), new DefaulProvider(settings.ProviderSettings, new FileHelper()))
        {
        }

        /// <summary>
        /// Create Engine instance with custom file provider and default parser.
        /// </summary>
        /// <param name="parserSettings">Parser settings.</param>
        /// <param name="fileProvider">Custom file provider instance.</param>
        public Engine(IParserSettings parserSettings, IFileProvider fileProvider)
            : this(new Parser((parserSettings?.Delimiter?.Value).GetValueOrDefault(',')), fileProvider)
        {
        }

        /// <summary>
        /// Get all single fixed format lines from a text file parsed into a strongly typed array
        /// Default delimiter is '|'. Override by specifying the delimiter in parser settings.
        /// Example File -
        /// "John Walsh|456RT4|True|Male"
        /// "Simone Walsh|456RT5|True|Female"
        /// </summary>
        /// <typeparam name="T">Typed Line Class</typeparam>
        /// <returns>
        /// Collection of Files each parsed with typed class arrays
        /// </returns>
        public File<T>[] GetFiles<T>() where T : FileLine, new()
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

                Data = parser.Parse<T>(file.Lines)
            }).ToArray();
        }

        /// <summary>
        /// Get all multi-format lines from a text file parsed into header, data and footer
        /// typed arrays respectively.
        /// Default delimiter is '|'.
        /// By default, Header line starts with H, data line starts with D and footer line starts with F.
        /// Override these values in parser settings.
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
        public File<TH, TD, TF>[] GetFiles<TH, TD, TF>()
            where TH : PrefixedFileLine, new()
            where TD : PrefixedFileLine, new()
            where TF : PrefixedFileLine, new()
        {
            var files = fileProvider.GetFiles();

            return files.Select(file =>
            {
                var headerLines = file.Lines.Where(line => line.StartsWith(new TH().Prefix)).ToArray();
                var footerLines = file.Lines.Where(line => line.StartsWith(new TF().Prefix)).ToArray();
                var dataLines = file.Lines.Where(line => line.StartsWith(new TD().Prefix)).ToArray();

                var parsed = new File<TH, TD, TF>
                {
                    FileMeta = new FileMeta
                    {
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                        FileSize = file.FileSize,
                        Lines = file.Lines,
                    },

                    Header = parser.Parse<TH>(headerLines).FirstOrDefault(),
                    Data = parser.Parse<TD>(dataLines).ToArray(),
                    Footer = parser.Parse<TF>(footerLines).FirstOrDefault()
                };

                return parsed;
            }).ToArray();
        }
    }
}