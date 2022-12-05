using TempoWorklogger.Model.UI;

namespace TempoWorklogger.Contract.UI.ViewModels.HomePage
{
    public interface IHomePageViewModel : IBaseViewModel, IDisposable
    {

        List<WorklogDraft> DraftWorklogs { get; }

        Dictionary<string, List<Model.Db.Worklog>> Worklogs { get; }

        ICommand<WorklogDraft> CreateDraftCommand { get; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync<WorklogDraft> CompleteDraftWorklogCommand { get; } 
    }
}
