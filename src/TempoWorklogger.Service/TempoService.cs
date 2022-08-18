using TempoWorklogger.Contract.Services;
using TempoWorklogger.Contract.Services.Tempo;
using TempoWorklogger.Service.Tempo;

namespace TempoWorklogger.Service
{
    public class TempoService : ITempoService
    {
        public IWorklogService WorklogService { get; }

        public TempoService(string accessToken)
        {
            WorklogService = new WorklogService(accessToken);
        }
    }
}