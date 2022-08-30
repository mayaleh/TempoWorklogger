using MediatR;
using Microsoft.AspNetCore.Components;
using System.Web;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Templates;
using TempoWorklogger.CQRS.Template.Queries;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;
using Command = TempoWorklogger.UI.Core.Command;

namespace TempoWorklogger.ViewModels.Templates
{
    public sealed class TemplatesViewModel : BaseViewModel, ITemplatesViewModel // maybe extract as abstract GridDataViewModel<TModel> with the CRUD commands
    {
        private readonly NavigationManager navigationManager;

        public TemplatesViewModel(IMediator mediator, NavigationManager navigationManager, Action onUiChanged) : base(mediator, onUiChanged)
        {
            // TODO commands and actions
            LoadCommand = new CommandAsync(Load);
            CreateCommand = new Command(Create);
            EditCommand = new TempoWorklogger.UI.Core.Command<string>(Edit);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            CreateCommand!.OnExecuteChanged += this.CreateCommand_OnExecuteChanged;
            EditCommand!.OnExecuteChanged += this.EditCommand_OnExecuteChanged;
            this.navigationManager = navigationManager;
        }

        public List<ImportMap> Templates { get; private set; }

        public ICommandAsync LoadCommand { get; }

        public ICommandAsync<string> DeleteCommand { get; }

        public ICommand<string> EditCommand { get; }

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
            var result = await this.Mediator.Send(new GetImportMapsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                //this.notifyMessage?.Error(result.ErrMessage);
            }

            Templates = result.Success?.ToList() ?? new List<ImportMap>();

            return Maya.Ext.Unit.Default;
        }

        private Maya.Ext.Unit Create()
        {
            this.navigationManager.NavigateTo("/templates/create");
            return Maya.Ext.Unit.Default;
        }

        private Maya.Ext.Unit Edit(string name)
        {
            var friendlyName = HttpUtility.UrlEncodeUnicode(name);
            this.navigationManager.NavigateTo($"/templates/{friendlyName}/edit");
            return Maya.Ext.Unit.Default;
        }
    }
}
