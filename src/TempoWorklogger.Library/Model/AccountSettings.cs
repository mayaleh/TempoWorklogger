namespace TempoWorklogger.Library.Model
{
    internal class AccountSettings
    {
        public string Name { get; set; }

        public string ApiKey { get; set; }

        public string UserId { get; set; }

        public Dictionary<string, string> Attributes { get; set; }
    }
}
