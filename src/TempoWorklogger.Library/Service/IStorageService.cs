using TempoWorklogger.Library.Service.Storage;

namespace TempoWorklogger.Library.Service
{
    public interface IStorageService
    {
        ImportMapTemplateService ImportMapTemplate { get; }
    }
}