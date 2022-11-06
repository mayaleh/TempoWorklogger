using SQLite;
using SQLiteNetExtensions.Attributes;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Model.Db
{
    public class Worklog : ICloneable
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        /// <summary>
        /// Jira Account ID of the author, maybe should be part of Integration setting model
        /// </summary>
        //[MaxLength(378)]
        //public string? AuthorAccountId { get; set; }

        /// <summary>
        /// Worklog description
        /// </summary>
        [MaxLength(378)]
        public string? Description { get; set; }

        /// <summary>
        /// Jira Issue key
        /// </summary>
        [MaxLength(255)]
        public string? IssueKey { get; set; }

        [Ignore]
        public DateTime StartDate { get => StartTime.Date; }

        public DateTime StartTime { get; set; }

        public int TimeSpentSeconds { get; set; } // is it required?

        [Ignore]
        public DateTime EndDate { get => EndTime.Date; }

        public DateTime EndTime { get; set; }

        [OneToMany]
        public ICollection<CustomAttributeKeyVal> Attributes { get; set; } = new List<CustomAttributeKeyVal>();

        [OneToMany]
        public ICollection<WorklogLog> Logs { get; set; } = new List<WorklogLog>();

        public object Clone()
        {
            var duplicatesOf = (Model.Db.Worklog)this.MemberwiseClone();
            duplicatesOf.Id = default;
            duplicatesOf.Logs = new List<WorklogLog>();
            return duplicatesOf;
        }
    }
}
