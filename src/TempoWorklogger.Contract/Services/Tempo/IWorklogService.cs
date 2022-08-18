using Maya.Ext.Rop;
using TempoWorklogger.Dto.Tempo;

namespace TempoWorklogger.Contract.Services.Tempo
{
    public interface IWorklogService
    {
        Task<Result<WorklogResponse, Exception>> Create(Worklog worklog);
    }
}
