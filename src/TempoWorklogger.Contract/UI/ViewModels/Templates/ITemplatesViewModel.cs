using TempoWorklogger.Dto.Storage;

namespace TempoWorklogger.Contract.UI.ViewModels.Templates
{
    public interface ITemplatesViewModel : IBaseViewModel, IDisposable
    {
        List<ImportMap> Templates { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync<string> DeleteCommand { get; }

        ICommand<string> EditCommand { get; }

        ICommand CreateCommand { get; }
    }
}
