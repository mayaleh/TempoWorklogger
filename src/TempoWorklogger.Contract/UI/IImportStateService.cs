using Maya.Ext.Rop;
using TempoWorklogger.Dto.Storage;
using TempoWorklogger.Dto.Tempo;
using FileInfo = TempoWorklogger.Dto.UI.FileInfo;

namespace TempoWorklogger.Contract.UI
{
    public interface IImportStateService : IDisposable
    {
        FileInfo File { get; set; }

        ImportMap ImportMap { get; set; }

        List<Result<Worklog, (Exception Exception, int RowNr)>> WorklogsResults { get; set; }

        List<Result<(Worklog, WorklogResponse), (Worklog, Exception)>> WorklogResponseResults { get; set; }
    }
}
