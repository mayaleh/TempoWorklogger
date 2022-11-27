using SQLite;
using SQLiteNetExtensions.Attributes;

namespace TempoWorklogger.Model.Db
{
    public class WorklogLog
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }

        public WorklogLogType Type { get; set; }
        
        public LogSeverity Severity { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(378)]
        public string? Message { get; set; }
        
        public string? Data { get; set; }

        [ForeignKey(typeof(Worklog))]
        public long WorklogId { get; set; }

        [ManyToOne]
        public Worklog Worklog { get; set; } = null!;
    }

    public record WorklogLogSendToTempoData(Worklog OriginalData, string Endpoint, Model.Tempo.WorklogResponse? Response);
}
