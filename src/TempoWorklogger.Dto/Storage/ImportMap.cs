namespace TempoWorklogger.Dto.Storage
{
    /// <summary>
    /// Import settings template
    /// </summary>
    public class ImportMap
    {
        public string Name { get; set; } = null!;

        /// <summary>
        /// from this row will read for example to skip header
        /// </summary>
        public int StartFromRow { get; set; }

        /// <summary>
        /// Tempo API access token generated in the Tempo app
        /// </summary>
        public string AccessToken { get; set; } = null!;

        /// <summary>
        /// Columns definitions for mapping data to json Tempo schema
        /// </summary>
        public ICollection<ColumnDefinition> ColumnDefinitions { get; set; } = new List<ColumnDefinition>();
    }
}
