namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardCommands : IDisposable
    {
        ICommandAsync NextStepCommand { get; }

        ICommandAsync PreviousStepCommand { get; }

        ICommandAsync ReadFileContentCommand { get; }

        ICommandAsync CancelImportCommand { get; }
    }
}
