namespace TempoWorklogger.Dto.Storage
{
    public class ColumnDefinition
    {
        public string Name { get; set; } = null!;

        public string Position { get; set; } = null!;

        public bool IsStatic { get; set; }

        public string Value { get; set; } = null!;

        public string Format { get; set; } = null!;
    }
}
