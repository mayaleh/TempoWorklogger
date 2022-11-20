using System.Text.Json.Serialization;

namespace TempoWorklogger.Model.Tempo
{
    public class Worklog
    {
        public string? AuthorAccountId { get; set; }
        
        public string? Description { get; set; }

        public string? IssueKey { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly StartDate { get; set; }

        [JsonConverter(typeof(TimeOnlyJsonConvertor))]
        public TimeOnly StartTime { get; set; }

        public int TimeSpentSeconds { get; set; }

        public ICollection<AttributeKeyVal> Attributes { get; set; } = new List<AttributeKeyVal>();
    }
}
