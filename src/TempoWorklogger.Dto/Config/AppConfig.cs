using TempoWorklogger.Contract.Config;

namespace TempoWorklogger.Dto.Config
{
    public class AppConfig : IAppConfig
    {
        public string DatabaseFileName { get; set; } = null!;
    }
}
