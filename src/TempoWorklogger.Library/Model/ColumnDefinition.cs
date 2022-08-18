namespace TempoWorklogger.Library.Model
{
    [Obsolete]
    public class ColumnDefinition
    {
        public string Name { get; set; }

        public string Position { get; set; }

        public bool IsStatic { get; set; }

        public string Value { get; set; }

        public string Format { get; set; }
    }
}
