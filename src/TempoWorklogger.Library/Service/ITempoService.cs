using Maya.Ext.Rop;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.Library.Service
{
    [Obsolete]
    public interface ITempoService
    {
        Task<Result<WorklogResponse, Exception>> CreateWorklog(Worklog worklog);
    }
}
