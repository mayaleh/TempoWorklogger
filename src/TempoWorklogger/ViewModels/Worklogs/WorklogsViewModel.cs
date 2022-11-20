using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;
using Command = TempoWorklogger.UI.Core.Command;

namespace TempoWorklogger.ViewModels.Worklogs
{
    public sealed class WorklogsViewModel : BaseViewModel, IWorklogsViewModel
    {
        public readonly NavigationManager NavigationManager;

        private readonly WorklogsActions worklogsActions;

        public WorklogsViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            NavigationManager navigationManager,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            worklogsActions = new WorklogsActions(this);

            LoadCommand = new CommandAsync(worklogsActions.Load);
            CreateDetailedCommand = new Command(worklogsActions.CreateDetailed);
            EditCommand = new TempoWorklogger.UI.Core.Command<long>(worklogsActions.Edit);
            PrepareDeleteCommand = new TempoWorklogger.UI.Core.Command<Model.Db.Worklog>(worklogsActions.PrepareDeletion);
            DeleteCommand = new CommandAsync(worklogsActions.Delete);

            CreateInlineCommand = new TempoWorklogger.UI.Core.CommandAsync<Model.Db.Worklog>(worklogsActions.CreateInline);
            UpdateInlineCommand = new TempoWorklogger.UI.Core.CommandAsync<Model.Db.Worklog>(worklogsActions.UpdateInline);

            SendSelectedToApiCommand = new CommandAsync<Model.Db.IntegrationSettings>(worklogsActions.SendSelectedToTempoApi);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            CreateDetailedCommand!.OnExecuteChanged += this.CreateDetailedCommand_OnExecuteChanged;
            EditCommand!.OnExecuteChanged += this.EditCommand_OnExecuteChanged;
            DeleteCommand!.OnExecuteChanged += this.DeleteCommand_OnExecuteChanged;

            CreateInlineCommand!.OnExecuteChanged += this.CreateInlineCommand_OnExecuteChanged;
            UpdateInlineCommand!.OnExecuteChanged += this.UpdateInlineCommand_OnExecuteChanged;
            NavigationManager = navigationManager;
        }

        public List<Model.Db.Worklog> Worklogs { get; set; } = new();

        public IList<Model.Db.Worklog> SelectedWorklogs { get; set; } = new List<Model.Db.Worklog>();

        public IList<Model.Db.IntegrationSettings> IntegrationSettingsList { get; set; } = new List<Model.Db.IntegrationSettings>();

        public ICommandAsync LoadCommand { get; }

        public ICommandAsync DeleteCommand { get; }

        public ICommand<long> EditCommand { get; }

        public ICommand CreateDetailedCommand { get; }

        public ICommand<Model.Db.Worklog> PrepareDeleteCommand { get; }

        public ICommandAsync<Model.Db.Worklog> CreateInlineCommand { get; }

        public ICommandAsync<Model.Db.Worklog> UpdateInlineCommand { get; }

        public ICommandAsync<Model.Db.IntegrationSettings> SendSelectedToApiCommand { get; } // set type

        public Action<int> OnProgressChanged { get; set; }

        public void Dispose()
        {
            if (this.LoadCommand != null)
            {
                this.LoadCommand.OnExecuteChanged -= LoadCommand_OnExecuteChanged;
            }
            if (this.CreateDetailedCommand != null)
            {
                this.CreateDetailedCommand.OnExecuteChanged -= CreateDetailedCommand_OnExecuteChanged;
            }
            if (this.EditCommand != null)
            {
                this.EditCommand.OnExecuteChanged -= EditCommand_OnExecuteChanged;
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
        private void CreateDetailedCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void CreateInlineCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void EditCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void UpdateInlineCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void DeleteCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;

    }
}
