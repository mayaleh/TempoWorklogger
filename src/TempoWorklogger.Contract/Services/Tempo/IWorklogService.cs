using Maya.Ext.Rop;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Contract.Services.Tempo
{
    public interface IWorklogService
    {
        Task<Result<WorklogResponse, Exception>> Create(Worklog worklog);
    }
}
