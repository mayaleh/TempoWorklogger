namespace TempoWorklogger.Contract.UI.ViewModels.Worklogs
{
    public interface IWorklogsViewModel : IBaseViewModel, IDisposable
    {
        List<Model.Db.Worklog> Worklogs { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync DeleteCommand { get; }

        ICommand<Model.Db.Worklog> PrepareDeleteCommand { get; }

        ICommand<long> EditCommand { get; }

        ICommand CreateDetailedCommand { get; }

        ICommandAsync<Model.Db.Worklog> CreateInlineCommand { get; }

        ICommandAsync<Model.Db.Worklog> UpdateInlineCommand { get; }
    }
}
