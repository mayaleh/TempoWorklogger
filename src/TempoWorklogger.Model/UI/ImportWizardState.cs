using Maya.Ext.Rop;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Model.UI
{
    public class ImportWizardState : IDisposable
    {
        public ImportWizardStepKind CurrentStep { get; set; } = ImportWizardStepKind.File;

        public FileInfo File { get; set; } = null!;

        public ImportMap SelectedImportMap { get; set; } = null!;

        public List<Result<Worklog, (Exception Exception, int RowNr)>> WorklogsFileResults { get; set; } = null!;

        public List<Result<Worklog, Exception>> WorklogsDbResults { get; set; } = null!;

        public List<Result<(Worklog, Tempo.WorklogResponse), (Worklog, Exception)>> WorklogTempoResponseResults { get; set; } = new();

        public void Dispose()
        {
            File = new();
            SelectedImportMap = new();
            WorklogsFileResults = new();
            WorklogsDbResults = new();
            WorklogTempoResponseResults = new();
        }
    }
}
