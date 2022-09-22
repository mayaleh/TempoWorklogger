using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    public class ImportWizardCommands : IImportWizardCommands
    {
        private readonly IImportWizardViewModel viewModel;

        public ImportWizardCommands(ImportWizardViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitCommands();
        }

        private void InitCommands()
        {
            NextStepCommand = new CommandAsync(this.viewModel.Actions.NextStep);
            PreviousStepCommand = new CommandAsync(this.viewModel.Actions.PreviousStep);
            CancelImportCommand = new CommandAsync(this.viewModel.Actions.CancelImport);
        }

        public ICommandAsync NextStepCommand { get; private set; }

        public ICommandAsync PreviousStepCommand { get; private set; }

        public ICommandAsync CancelImportCommand { get; private set; }
    }
}
