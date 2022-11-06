using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TempoWorklogger.Model.Db
{
    public class ImportMap
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the import
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Start read the import from row (typically from excel or csv row)
        /// </summary>
        public int StartFromRow { get; set; }

        /// <summary>
        /// Import for file type (csv or xlsx)
        /// </summary>
        public FileTypeKinds FileType { get; set; } = FileTypeKinds.Xlsx;

        /// <summary>
        /// Columns map definitions
        /// </summary>
        [OneToMany]
        public ICollection<ColumnDefinition> ColumnDefinitions { get; set; } = new List<ColumnDefinition>();
    }
}
