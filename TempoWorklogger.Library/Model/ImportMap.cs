namespace TempoWorklogger.Library.Model
{
    /// <summary>
    /// Import settings template
    /// </summary>
    public class ImportMap
    {
        public string Name { get; set; }

        /// <summary>
        /// from this row will read for example to skip header
        /// </summary>
        public int StartFromRow { get; set; }

        public string AccessToken { get; set; }

        public ICollection<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
