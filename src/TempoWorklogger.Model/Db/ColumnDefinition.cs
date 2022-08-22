using SQLite;

namespace TempoWorklogger.Model.Db
{
    public class ColumnDefinition //: IColumnDefinition
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; } = null!;

        [MaxLength(3)]
        public string Position { get; set; } = null!;

        public bool IsStatic { get; set; }

        [MaxLength(512)]
        public string Value { get; set; } = null!;
        
        [MaxLength(48)]
        public string Format { get; set; } = null!;
    }
}
