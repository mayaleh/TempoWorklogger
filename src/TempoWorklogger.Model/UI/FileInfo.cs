namespace TempoWorklogger.Model.UI
{
    public class FileInfo
    {
        public MemoryStream Content { get; set; } = new MemoryStream();

        public string ContentType { get; set; } = null!;

        public string Name { get; set; } = null!;

        public long Size { get; set; }

        public DateTimeOffset LastModified { get; set; }
    }
}
