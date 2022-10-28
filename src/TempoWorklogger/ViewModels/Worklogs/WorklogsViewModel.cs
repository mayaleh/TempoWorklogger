using Maya.Ext.Func.Rop;
using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;
using TempoWorklogger.CQRS.Worklogs.Queries;
using TempoWorklogger.UI.Core;
using Command = TempoWorklogger.UI.Core.Command;

namespace TempoWorklogger.ViewModels.Worklogs
{
    using unitResult = Maya.Ext.Rop.Result<Maya.Ext.Unit, Exception>;

    public sealed class WorklogsViewModel : BaseViewModel, IWorklogsViewModel
    {
        private readonly NavigationManager navigationManager;

        private Model.Db.Worklog worklogToBeDeleted = new();

        public WorklogsViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            NavigationManager navigationManager,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            LoadCommand = new CommandAsync(Load);
            CreateCommand = new Command(Create);
            EditCommand = new TempoWorklogger.UI.Core.Command<long>(Edit);
            PrepareDeleteCommand = new TempoWorklogger.UI.Core.Command<Model.Db.Worklog>(PrepareDeletion);
            DeleteCommand = new CommandAsync(Delete);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            CreateCommand!.OnExecuteChanged += this.CreateCommand_OnExecuteChanged;
            EditCommand!.OnExecuteChanged += this.EditCommand_OnExecuteChanged;
            DeleteCommand!.OnExecuteChanged += this.DeleteCommand_OnExecuteChanged;
            this.navigationManager = navigationManager;
        }

        public List<Model.Db.Worklog> Worklogs { get; private set; }

        public ICommandAsync LoadCommand { get; }

        public ICommandAsync DeleteCommand { get; }

        public ICommand<long> EditCommand { get; }

        public ICommand CreateCommand { get; }

        public ICommand<Model.Db.Worklog> PrepareDeleteCommand { get; }

        public void Dispose()
        {
            if (this.LoadCommand != null)
            {
                this.LoadCommand.OnExecuteChanged -= LoadCommand_OnExecuteChanged;
            }
            if (this.CreateCommand != null)
            {
                this.CreateCommand.OnExecuteChanged -= CreateCommand_OnExecuteChanged;
            }
            if (this.EditCommand != null)
            {
                this.EditCommand.OnExecuteChanged -= EditCommand_OnExecuteChanged;
            }
            if (this.DeleteCommand != null)
            {
                this.DeleteCommand.OnExecuteChanged -= DeleteCommand_OnExecuteChanged;
            }
        }

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void CreateCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void EditCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void DeleteCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;

        private async Task<Maya.Ext.Unit> Load()
        {
            var result = await this.Mediator.Send(new GetWorklogsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.NotificationService.ShowError(result.Failure.Message);
            }

            Worklogs = result.Success?.ToList() ?? new List<Model.Db.Worklog>();

            return Maya.Ext.Unit.Default;
        }

        private void Create()
        {
            this.navigationManager.NavigateTo("/worklog/create");
        }

        private Maya.Ext.Unit Edit(long id)
        {
            this.navigationManager.NavigateTo($"/worklog/edit/{id}");
            return Maya.Ext.Unit.Default;
        }

        private Maya.Ext.Unit PrepareDeletion(Model.Db.Worklog toBeDeleted)
        {
            this.worklogToBeDeleted = toBeDeleted;
            return Maya.Ext.Unit.Default;
        }

        private async Task<Maya.Ext.Unit> Delete()
        {
            if (worklogToBeDeleted.Id == default(int))
            {
                await NotificationService.ShowError("Worklog is not defined.");
                return Maya.Ext.Unit.Default;
            }

            return await Mediator.Send(new CQRS.Worklogs.Commands.DeleteCommand(this.worklogToBeDeleted))
                .EitherAsync(
                    async success =>
                    {
                        await NotificationService.ShowSuccess($"Successfully deleted the worklog.");
                        worklogToBeDeleted = new();
                        await Load();
                        return unitResult.Succeeded(Maya.Ext.Unit.Default);
                    },
                    async failure =>
                    {
                        Console.WriteLine(failure.Message);
                        await NotificationService.ShowError($"Failled to delete th worklog. Message: {failure.Message}");
                        return unitResult.Failed(failure);
                    }
                ).ValueOrAsync(Maya.Ext.Unit.Default);
        }
    }
}
