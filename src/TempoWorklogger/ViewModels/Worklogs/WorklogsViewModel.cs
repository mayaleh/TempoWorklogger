using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;
using TempoWorklogger.CQRS.Worklogs.Queries;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;
using Command = TempoWorklogger.UI.Core.Command;

namespace TempoWorklogger.ViewModels.Worklogs
{
    public sealed class WorklogsViewModel : BaseViewModel, IWorklogsViewModel
    {
        private readonly NavigationManager navigationManager;

        public WorklogsViewModel(IMediator mediator, NavigationManager navigationManager, Action onUiChanged) : base(mediator, onUiChanged)
        {
            // TODO Delete command
            LoadCommand = new CommandAsync(Load);
            CreateCommand = new Command(Create);
            EditCommand = new TempoWorklogger.UI.Core.Command<long>(Edit);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            CreateCommand!.OnExecuteChanged += this.CreateCommand_OnExecuteChanged;
            EditCommand!.OnExecuteChanged += this.EditCommand_OnExecuteChanged;
            this.navigationManager = navigationManager;
        }

        public List<Worklog> Worklogs { get; private set; }

        public ICommandAsync LoadCommand { get; }

        public ICommandAsync<long> DeleteCommand { get; }

        public ICommand<long> EditCommand { get; }

        public ICommand CreateCommand { get; }

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
        }

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void CreateCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void EditCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;

        private async Task<Maya.Ext.Unit> Load()
        {
            var result = await this.Mediator.Send(new GetWorklogsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                //this.notifyMessage?.Error(result.ErrMessage);
            }

            Worklogs = result.Success?.ToList() ?? new List<Worklog>();

            return Maya.Ext.Unit.Default;
        }

        private void Create()
        {
            this.navigationManager.NavigateTo("/worklogs/create");
            //return Maya.Ext.Unit.Default;
        }

        private Maya.Ext.Unit Edit(long id)
        {
            this.navigationManager.NavigateTo($"/worklogs/{id}/edit");
            return Maya.Ext.Unit.Default;
        }

    }
}
