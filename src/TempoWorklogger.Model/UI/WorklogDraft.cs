using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Model.UI
{
    public class WorklogDraft : IIntervalNullable, ICloneable
    {
        /// <summary>
        /// Worklog description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Jira Issue key
        /// </summary>

        public string? IssueKeyAndTitle => IssueKey == null
            ? null
            : IssueKey + (Title == null ? "" : " - " + Title);


        public string? IssueKey { get; set; }

        public string? Title { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public object Clone()
        {
            return (WorklogDraft)this.MemberwiseClone();
        }
    }
}
