namespace Ninja.FileUtil
{
    public class File<T> where T: FileLine
    {
        public FileMeta FileMeta { get; set; }
        public T[] Data { get; set; }
       
    }

    public class File<TH, TD, TF> where TH : FileLine, new()
                                  where TD : FileLine, new()
                                  where TF : FileLine, new()
    {
        public FileMeta FileMeta { get; set; }
        public TH[] Headers { get; set; }
        public TD[] Data { get; set; }
        public TF[] Footers { get; set; }
    }

    public class FileMeta
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string[] Lines { get; set; }
    }
}