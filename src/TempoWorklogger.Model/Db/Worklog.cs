using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TempoWorklogger.Model.Db
{
    public class Worklog
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        /// <summary>
        /// Jira Account ID of the author
        /// </summary>
        [MaxLength(378)]
        public string? AuthorAccountId { get; set; }

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

        public DateOnly StartDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public int TimeSpentSeconds { get; set; }

        public TimeOnly EndTime { get; set; }

        [OneToMany]
        public ICollection<CustomAttributeKeyVal> Attributes { get; set; } = new List<CustomAttributeKeyVal>();
    }
}
