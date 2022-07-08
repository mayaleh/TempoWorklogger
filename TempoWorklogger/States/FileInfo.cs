namespace TempoWorklogger.States
{
    public class FileInfo
    {
        public MemoryStream Content { get; set; } = new MemoryStream();
        public string ContentType { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTimeOffset LastModified { get; set; }
    }
}
