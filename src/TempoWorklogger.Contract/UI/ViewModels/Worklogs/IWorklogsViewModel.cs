using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Worklogs
{
    public interface IWorklogsViewModel : IBaseViewModel, IDisposable
    {
        List<Worklog> Worklogs { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync<long> DeleteCommand { get; }

        ICommand<long> EditCommand { get; }

        ICommand CreateCommand { get; }
    }
}
