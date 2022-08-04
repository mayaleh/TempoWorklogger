using Maya.Ext.Rop;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.Library.Service
{
    public interface ITempoService
    {
        Task<Result<WorklogResponse, Exception>> CreateWorklog(Worklog worklog);
    }
}
