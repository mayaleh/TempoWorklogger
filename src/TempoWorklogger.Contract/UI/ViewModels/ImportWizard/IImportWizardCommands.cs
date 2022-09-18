namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardCommands
    {
        ICommandAsync NextStepCommand { get; }

        ICommandAsync PreviousStepCommand { get; }

        ICommandAsync CancelImportCommand { get; }
    }
}
