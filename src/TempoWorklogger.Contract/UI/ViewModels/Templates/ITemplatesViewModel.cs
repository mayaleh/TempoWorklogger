using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Templates
{
    public interface ITemplatesViewModel : IBaseViewModel, IDisposable
    {
        List<ImportMap> Templates { get; }

        ICommandAsync LoadCommand { get; }

        ICommand<ImportMap> PrepareDeleteCommand { get; }

        ICommandAsync DeleteCommand { get; }

        ICommand<string> EditCommand { get; }

        ICommand CreateCommand { get; }
    }
}
