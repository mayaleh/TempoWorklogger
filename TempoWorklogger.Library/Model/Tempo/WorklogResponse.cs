namespace TempoWorklogger.Library.Model.Tempo
{
    public class Result<T>
    {
        public Metadata Metadata { get; set; }
        public IEnumerable<T> Results { get; set; }
        public string Self { get; set; }
    }

    public class Metadata
    {
        public int Count { get; set; }
        public int Limit { get; set; }
        public string Next { get; set; }
        public int Offset { get; set; }
        public string Previous { get; set; }
    }

    public class WorklogResponse
    {
        public Attributes Attributes { get; set; }
        public Author Author { get; set; }
        public int BillableSeconds { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public Issue Issue { get; set; }
        public int JiraWorklogId { get; set; }
        public string Self { get; set; }
        public string StartDate { get; set; }
        public int StartTime { get; set; }
        public int TempoWorklogId { get; set; }
        public int TimeSpentSeconds { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class Attributes
    {
        public string Self { get; set; }
        public object[] Values { get; set; }
    }

    public class Author
    {
        public string AccountId { get; set; }
        public string Self { get; set; }
    }

    public class Issue
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Self { get; set; }
    }

}
