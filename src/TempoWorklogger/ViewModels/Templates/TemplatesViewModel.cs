using MediatR;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Templates;
using TempoWorklogger.Dto.Storage;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Templates
{
    public class TemplatesViewModel : BaseViewModel, ITemplatesViewModel
    {
        public TemplatesViewModel(IMediator mediator, Action onUiChanged) : base(mediator, onUiChanged)
        {
            // TODO commands and actions
        }

        public List<ImportMap> Templates { get; }

        public ICommandAsync LoadCommand { get; }

        public ICommandAsync<string> DeleteCommand { get; }

        public ICommand<string> EditCommand { get; }

        public ICommand CreateCommand { get; }

        public void Dispose()
        {

        }
    }
}
