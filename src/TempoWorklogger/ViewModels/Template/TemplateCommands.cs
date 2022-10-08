using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Template;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Template
{
    internal class TemplateCommands : ITemplateCommands
    {
        private readonly ITemplateViewModel viewModel;

        public TemplateCommands(ITemplateViewModel viewModel)
        {
            this.viewModel = viewModel;

            InitCommands();
        }

        private void InitCommands()
        {
            LoadCommand = new CommandAsync<int?>(viewModel.Actions.Load);
            SaveCommand = new CommandAsync(viewModel.Actions.Save);
            AddAttributeCommand = new CommandAsync(viewModel.Actions.AddAttribute);
            RemoveAttributeCommand = new CommandAsync<Model.Db.ColumnDefinition>(viewModel.Actions.RemoveAttribute);


            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
        }

        /// <inheritdoc/>
        public ICommandAsync<int?> LoadCommand { get; private set; }

        /// <inheritdoc/>
        public ICommandAsync SaveCommand { get; private set; }

        /// <inheritdoc/>
        public ICommandAsync AddAttributeCommand { get; private set; }

        /// <inheritdoc/>
        public ICommandAsync<Model.Db.ColumnDefinition> RemoveAttributeCommand { get; private set; }

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;
        
        private void SaveCommand_OnExecuteChanged(object sender, bool e) => this.viewModel.IsBusy = e;

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
        }
    }
}
