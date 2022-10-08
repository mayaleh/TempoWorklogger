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

        private ImportMap toBeDeleted = new();

        public TemplatesViewModel(IMediator mediator, NavigationManager navigationManager, Action onUiChanged) : base(mediator, onUiChanged)
        {
            // TODO commands and actions
            LoadCommand = new CommandAsync(Load);
            CreateCommand = new Command(Create);
            EditCommand = new TempoWorklogger.UI.Core.Command<string>(Edit);
            PrepareDeleteCommand = new TempoWorklogger.UI.Core.Command<ImportMap>(DeletePrepare);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
            CreateCommand!.OnExecuteChanged += this.CreateCommand_OnExecuteChanged;
            EditCommand!.OnExecuteChanged += this.EditCommand_OnExecuteChanged;
            this.navigationManager = navigationManager;
        }

        public List<ImportMap> Templates { get; private set; } = new List<ImportMap>();

        public ICommandAsync LoadCommand { get; }

        public ICommand<string> EditCommand { get; }

        public ICommand CreateCommand { get; }

        public ICommand<ImportMap> PrepareDeleteCommand { get; }

        ICommandAsync ITemplatesViewModel.DeleteCommand { get; }

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
            //this.IsInit = true;
            var result = await this.Mediator.Send(new GetImportMapsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                //this.notifyMessage?.Error(result.ErrMessage);
            }

            Templates = result.Success?.ToList() ?? new List<ImportMap>();

            //this.IsInit = false;
            return Maya.Ext.Unit.Default;
        }

        private void Create()
        {
            this.navigationManager.NavigateTo("/template/create");
            //return Maya.Ext.Unit.Default;
        }

        private Maya.Ext.Unit DeletePrepare(ImportMap importMap)
        {
            this.toBeDeleted = importMap;
            return Maya.Ext.Unit.Default;
        }

        private async Task DeleteExecutingConfirmed()
        {
            // TBD
        }

        private Maya.Ext.Unit Edit(string name)
        {
            var friendlyName = HttpUtility.UrlEncodeUnicode(name);
            this.navigationManager.NavigateTo($"/templates/{friendlyName}/edit");
            return Maya.Ext.Unit.Default;
        }
    }
}
