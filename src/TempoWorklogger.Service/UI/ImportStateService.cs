using Maya.Ext.Rop;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Dto.Storage;
using TempoWorklogger.Dto.Tempo;
using FileInfo = TempoWorklogger.Dto.UI.FileInfo;

namespace TempoWorklogger.Service.UI
{
    public class ImportStateService : IImportStateService
    {
        public FileInfo File { get; set; } = null!;

        public ImportMap ImportMap { get; set; } = null!;

        public List<Result<Worklog, (Exception Exception, int RowNr)>> WorklogsResults { get; set; } = null!;

        public List<Result<(Worklog, WorklogResponse), (Worklog, Exception)>> WorklogResponseResults { get; set; } = new();

        public void Dispose()
        {
            File = new();
            ImportMap = new();
            WorklogsResults = new();
            WorklogResponseResults = new();
        }
    }
}
