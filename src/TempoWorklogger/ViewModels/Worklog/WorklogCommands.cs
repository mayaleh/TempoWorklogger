using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Worklog;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Worklog
{
    public class WorklogCommands : IWorklogCommands
    {
        private readonly IWorklogViewModel viewModel;

        public WorklogCommands(IWorklogViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitCommands();
        }

        private void InitCommands()
        {
            LoadCommand = new CommandAsync<long?>(viewModel.Actions.Load);
            SaveCommand = new CommandAsync(viewModel.Actions.Save);
            AddAttributeCommand = new CommandAsync<CustomAttributeKeyVal>(viewModel.Actions.AddAttribute);
            RemoveAttributeCommand = new CommandAsync<CustomAttributeKeyVal>(viewModel.Actions.RemoveAttribute);

            LoadCommand.OnExecuteChanged += LoadCommand_OnExecuteChanged;
            SaveCommand.OnExecuteChanged += SaveCommand_OnExecuteChanged;
            AddAttributeCommand.OnExecuteChanged += AddAttributeCommand_OnExecuteChanged;
            RemoveAttributeCommand.OnExecuteChanged += RemoveAttributeCommand_OnExecuteChanged;
        }

        public ICommandAsync<long?> LoadCommand { get; private set; }

        public ICommandAsync SaveCommand { get; private set; }

        public ICommandAsync<CustomAttributeKeyVal> AddAttributeCommand { get; private set; }

        public ICommandAsync<CustomAttributeKeyVal> RemoveAttributeCommand { get; private set; }

        private void AddAttributeCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = this.viewModel.IsInit = e;

        private void SaveCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        private void RemoveAttributeCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

        public void Dispose()
        {
            if (this.LoadCommand != null)
            {
                this.LoadCommand.OnExecuteChanged -= LoadCommand_OnExecuteChanged;
            }
            if (this.SaveCommand != null)
            {
                this.SaveCommand.OnExecuteChanged -= SaveCommand_OnExecuteChanged;
            }
            if (this.AddAttributeCommand != null)
            {
                this.AddAttributeCommand.OnExecuteChanged -= AddAttributeCommand_OnExecuteChanged;
            }
            if (this.RemoveAttributeCommand != null)
            {
                this.RemoveAttributeCommand.OnExecuteChanged -= RemoveAttributeCommand_OnExecuteChanged;
            }
        }
    }
}
