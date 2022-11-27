using TempoWorklogger.Contract.UI.ViewModels.Common;

namespace TempoWorklogger.Contract.UI.ViewModels.Worklogs
{
    public interface IWorklogsViewModel : IBaseViewModel, IProgressViewModel, IDisposable
    {
        List<Model.Db.WorklogView> Worklogs { get; }

        List<Model.Db.Worklog> AutoCompleteWorklogs { get; }

        IList<Model.Db.WorklogView> SelectedWorklogs { get; set; }

        IList<Model.Db.IntegrationSettings> IntegrationSettingsList { get; set; }

        Dictionary<long, Maya.Ext.Rop.Result<Model.Tempo.WorklogResponse, Exception>> SentToTempoResults { get; }

        ICommandAsync<Model.Db.IntegrationSettings> SendSelectedToApiCommand { get; }

        ICommand StopSendingSelectedToApiCommand { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync DeleteCommand { get; }

        ICommand<Model.Db.WorklogView> PrepareDeleteCommand { get; }

        ICommand<long> EditCommand { get; }

        ICommand CreateDetailedCommand { get; }

        ICommandAsync ResetSendToTempoCommand { get; }

        ICommandAsync<Model.Db.WorklogView> CreateInlineCommand { get; }

        ICommandAsync<Model.Db.WorklogView> UpdateInlineCommand { get; }
    }
}
