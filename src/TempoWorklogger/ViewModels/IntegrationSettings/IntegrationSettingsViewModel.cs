using MediatR;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.IntegrationSettings;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.IntegrationSettings
{
    public class IntegrationSettingsViewModel : BaseViewModel, IIntegrationSettingsViewModel
    {
        private readonly IntegrationSettingsActions settingsActions;

        public IntegrationSettingsViewModel(IMediator mediator, IUINotificationService notificationService, Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            settingsActions = new IntegrationSettingsActions(this);

            LoadCommand = new CommandAsync(settingsActions.LoadIntegrationSettings);
            PrepareDeleteCommand = new TempoWorklogger.UI.Core.Command<Model.Db.IntegrationSettings>(settingsActions.PrepareDeletion);
            DeleteCommand = new CommandAsync(settingsActions.Delete);
            CreateInlineCommand = new TempoWorklogger.UI.Core.CommandAsync<Model.Db.IntegrationSettings>(settingsActions.CreateInline);
            UpdateInlineCommand = new TempoWorklogger.UI.Core.CommandAsync<Model.Db.IntegrationSettings>(settingsActions.UpdateInline);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            DeleteCommand!.OnExecuteChanged += this.DeleteCommand_OnExecuteChanged;
            CreateInlineCommand!.OnExecuteChanged += this.CreateInlineCommand_OnExecuteChanged;
            UpdateInlineCommand!.OnExecuteChanged += this.UpdateInlineCommand_OnExecuteChanged;
        }

        public List<Model.Db.IntegrationSettings> IntegrationSettingsList { get; set; }
        public ICommandAsync LoadCommand { get; }
        public ICommandAsync DeleteCommand { get; }
        public ICommand<Model.Db.IntegrationSettings> PrepareDeleteCommand { get; }
        public ICommandAsync<Model.Db.IntegrationSettings> CreateInlineCommand { get; }
        public ICommandAsync<Model.Db.IntegrationSettings> UpdateInlineCommand { get; }

        public void Dispose()
        {
            if (this.LoadCommand != null)
            {
                this.LoadCommand.OnExecuteChanged -= LoadCommand_OnExecuteChanged;
            }
            if (this.DeleteCommand != null)
            {
                this.DeleteCommand.OnExecuteChanged -= DeleteCommand_OnExecuteChanged;
            }
            if (this.CreateInlineCommand != null)
            {
                this.CreateInlineCommand.OnExecuteChanged -= CreateInlineCommand_OnExecuteChanged;
            }
            if (this.UpdateInlineCommand != null)
            {
                this.UpdateInlineCommand.OnExecuteChanged -= UpdateInlineCommand_OnExecuteChanged;
            }
        }

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void CreateInlineCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void UpdateInlineCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void DeleteCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
    }
}
