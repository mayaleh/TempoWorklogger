using TempoWorklogger.Contract.Services.Tempo;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.Services
{
    public interface ITempoService
    {
        IWorklogService WorklogService { get; }
    }

    public interface ITempoServiceFactory
    {
        ITempoService CreateService(IntegrationSettings integrationSettings);
    }
}
