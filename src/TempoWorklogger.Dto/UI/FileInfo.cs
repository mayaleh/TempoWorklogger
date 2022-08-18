namespace TempoWorklogger.Dto.UI
{
    public class FileInfo
    {
        /// <summary>
        /// Content stream of the file
        /// </summary>
        public MemoryStream Content { get; set; } = new MemoryStream();

        /// <summary>
        /// Media type - content format of the loaded file
        /// </summary>
        public string ContentType { get; set; } = null!;

        /// <summary>
        /// Loaded file name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Size of the loaded file
        /// </summary>
        public long Size { get; set; }
        
        /// <summary>
        /// Last modified datetime of the loaded file
        /// </summary>
        public DateTimeOffset LastModified { get; set; }
    }
}
