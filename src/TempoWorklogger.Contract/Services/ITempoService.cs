using TempoWorklogger.Contract.Services.Tempo;

namespace TempoWorklogger.Contract.Services
{
    public interface ITempoService
    {
        IWorklogService WorklogService { get; }
    }
}
