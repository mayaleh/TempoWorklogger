using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.UI;

namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardViewModel
    {
        ImportWizardState ImportWizardState { get; }
        
        ICollection<ImportMap> ImportMappingTemplates { get; }

        IImportWizardActions Actions { get; }

        IImportWizardCommands Commands { get; }
    }
}
