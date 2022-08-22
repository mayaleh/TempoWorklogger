using MediatR;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Templates;
using TempoWorklogger.CQRS.Template.Queries;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Templates
{
    public sealed class TemplatesViewModel : BaseViewModel, ITemplatesViewModel // maybe extract as abstract GridDataViewModel<TModel> with the CRUD commands
    {
        public TemplatesViewModel(IMediator mediator, Action onUiChanged) : base(mediator, onUiChanged)
        {
            // TODO commands and actions
            LoadCommand = new CommandAsync(Load);

            LoadCommand!.OnExecuteChanged += this.LoadCommand_OnExecuteChanged;
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
        }

        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;

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
    }
}
