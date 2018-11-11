namespace Ninja.FileUtil
{
    public class File<T> where T: FileLine
    {
        /// <summary>
        /// File meta data.
        /// </summary>
        public FileMeta FileMeta { get; set; }
        /// <summary>
        /// Strongly typed parsed lines.
        /// </summary>
        public T[] Data { get; set; }
       
    }

    public class File<TH, TD, TF> where TH : FileLine, new()
                                  where TD : FileLine, new()
                                  where TF : FileLine, new()
    {
        /// <summary>
        /// File meta data.
        /// </summary>
        public FileMeta FileMeta { get; set; }
        /// <summary>
        /// Parsed header lines.
        /// </summary>
        public TH[] Headers { get; set; }
        /// <summary>
        /// Parsed data lines.
        /// </summary>
        public TD[] Data { get; set; }
        /// <summary>
        /// Parsed footer lines.
        /// </summary>
        public TF[] Footers { get; set; }
    }

    public class FileMeta
    {
        /// <summary>
        /// File Path
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// File Name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File Size
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// Raw lines in the file (not parsed).
        /// </summary>
        public string[] Lines { get; set; }
    }
}