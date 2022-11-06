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
            ReadFileContentCommand = new CommandAsync(this.viewModel.Actions.ReadFileContent);
            ExecuteImportCommand = new CommandAsync(this.viewModel.Actions.ExecuteImportToDb);

            NextStepCommand.OnExecuteChanged += NextStepCommand_OnExecuteChanged;
            PreviousStepCommand.OnExecuteChanged += PreviousStepCommand_OnExecuteChanged;
            ReadFileContentCommand.OnExecuteChanged += ReadFileContentCommand_OnExecuteChanged;
        }

        private void NextStepCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        private void PreviousStepCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        private void ReadFileContentCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        public void Dispose()
        {
            if (this.NextStepCommand != null)
            {
                this.NextStepCommand.OnExecuteChanged -= NextStepCommand_OnExecuteChanged;
            }
            if (this.PreviousStepCommand != null)
            {
                this.PreviousStepCommand.OnExecuteChanged -= PreviousStepCommand_OnExecuteChanged;
            }
            if (this.ReadFileContentCommand != null)
            {
                this.ReadFileContentCommand.OnExecuteChanged -= ReadFileContentCommand_OnExecuteChanged;
            }
        }

        public ICommandAsync NextStepCommand { get; private set; }

        public ICommandAsync PreviousStepCommand { get; private set; }

        public ICommandAsync CancelImportCommand { get; private set; }

        public ICommandAsync ReadFileContentCommand { get; private set; }

        public ICommandAsync ExecuteImportCommand { get; private set; }
    }
}
