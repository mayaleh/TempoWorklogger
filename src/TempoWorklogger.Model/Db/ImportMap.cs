using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TempoWorklogger.Model.Db
{
    /// <inheritdoc/>
    public class ImportMap //: IImportMap
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <inheritdoc/>
        [MaxLength(255)]
        public string Name { get; set; } = null!;

        /// <inheritdoc/>
        public int StartFromRow { get; set; }

        /// <inheritdoc/>
        [MaxLength(255)]
        public string AccessToken { get; set; } = null!;

        /// <inheritdoc/>
        public FileTypeKinds FileType { get; set; }

        /// <inheritdoc/>
        [OneToMany]
        public ICollection<ColumnDefinition> ColumnDefinitions { get; set; } = new List<ColumnDefinition>();
    }
}
