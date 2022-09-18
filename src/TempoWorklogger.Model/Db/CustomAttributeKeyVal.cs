using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TempoWorklogger.Model.Db
{
    public class CustomAttributeKeyVal
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        [MaxLength(255)]
        public string? Key { get; set; }

        public string? Value { get; set; }

        [ForeignKey(typeof(Worklog))]
        public long WorklogId { get; set; }

        [ManyToOne]
        public Worklog ImportMap { get; set; } = null!;
    }
}
