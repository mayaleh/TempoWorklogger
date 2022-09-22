using TempoWorklogger.Model.UI;

namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardViewModel
    {
        ImportWizardState ImportWizardState { get; }

        IImportWizardActions Actions { get; }

        IImportWizardCommands Commands { get; }
    }
}
