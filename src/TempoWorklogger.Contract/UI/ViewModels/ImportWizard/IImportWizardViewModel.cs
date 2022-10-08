using TempoWorklogger.Contract.UI.ViewModels.Common;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.UI;

namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardViewModel : IProgressViewModel, IBaseViewModel, IDisposable
    {
        ImportWizardState ImportWizardState { get; }

        IEnumerable<ImportMap> ImportMappingTemplates { get; }

        IImportWizardActions Actions { get; }

        IImportWizardCommands Commands { get; }

        string ErrorMessage { get; set; }
    }
}
