using TempoWorklogger.Contract.Services;
using TempoWorklogger.Contract.Services.Tempo;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Service.Tempo;

namespace TempoWorklogger.Service
{
    public class TempoService : ITempoService
    {
        public IWorklogService WorklogService { get; }

        public TempoService(IntegrationSettings integrationSettings)
        {
            WorklogService = new WorklogService(integrationSettings);
        }
    }

    public class TempoServiceFactory : ITempoServiceFactory
    {
        public ITempoService CreateService(IntegrationSettings integrationSettings)
        {
            return new TempoService(integrationSettings);
        }
    }
}