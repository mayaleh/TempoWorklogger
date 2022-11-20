using TempoWorklogger.Contract.UI.ViewModels.Common;

namespace TempoWorklogger.Contract.UI.ViewModels.Worklogs
{
    public interface IWorklogsViewModel : IBaseViewModel, IProgressViewModel, IDisposable
    {
        List<Model.Db.Worklog> Worklogs { get; }

        IList<Model.Db.Worklog> SelectedWorklogs { get; set; }

        IList<Model.Db.IntegrationSettings> IntegrationSettingsList { get; set; }

        ICommandAsync<Model.Db.IntegrationSettings> SendSelectedToApiCommand { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync DeleteCommand { get; }

        ICommand<Model.Db.Worklog> PrepareDeleteCommand { get; }

        ICommand<long> EditCommand { get; }

        ICommand CreateDetailedCommand { get; }

        ICommandAsync<Model.Db.Worklog> CreateInlineCommand { get; }

        ICommandAsync<Model.Db.Worklog> UpdateInlineCommand { get; }
    }
}
