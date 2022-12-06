using Maya.Ext.Func.Rop;
using Maya.Ext.Rop;
using MediatR;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.HomePage;
using TempoWorklogger.Model.UI;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.HomePage
{
    public class HomePageViewModel : BaseViewModel, IHomePageViewModel
    {
        public HomePageViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            CreateDraftCommand = new UI.Core.Command<WorklogDraft>(CreateDraft);
            CompleteDraftWorklogCommand = new CommandAsync<WorklogDraft>(CompleteDraftWorklog);
            LoadCommand = new CommandAsync(Load);

            CreateDraftCommand.OnExecuteChanged += CreateDraftWorklogCommand_OnExecuteChanged;
            CompleteDraftWorklogCommand.OnExecuteChanged += CompleteDraftWorklogCommand_OnExecuteChanged;
            LoadCommand.OnExecuteChanged += LoadCommand_OnExecuteChanged;
        }

        public void Dispose()
        {
            if (this.CreateDraftCommand != null)
            {
                this.CreateDraftCommand.OnExecuteChanged -= CreateDraftWorklogCommand_OnExecuteChanged;
            }
            if (this.CompleteDraftWorklogCommand != null)
            {
                this.CompleteDraftWorklogCommand.OnExecuteChanged -= CompleteDraftWorklogCommand_OnExecuteChanged;
            }
            if (this.LoadCommand != null)
            {
                this.LoadCommand.OnExecuteChanged -= LoadCommand_OnExecuteChanged;
            }
        }

        public List<WorklogDraft> DraftWorklogs { get; } = new List<WorklogDraft>();

        public Dictionary<string, List<Model.Db.Worklog>> Worklogs { get; private set; } = new();

        public ICommand<WorklogDraft> CreateDraftCommand { get; }

        public ICommandAsync<WorklogDraft> CompleteDraftWorklogCommand { get; }

        public ICommandAsync LoadCommand { get; }

        private void CreateDraftWorklogCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void CompleteDraftWorklogCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;
        private void LoadCommand_OnExecuteChanged(object sender, bool e) => this.IsBusy = e;

        private async Task<Maya.Ext.Unit> Load()
        {
            return await Mediator.Send(new CQRS.Worklogs.Queries.GetWorklogsQuery())
                .MatchSuccessAsync(success =>
                {
                    Worklogs = success.GroupBy(x => x.IssueKey)
                        .ToDictionary(g => g.Key + " - " + (g.First().Title ?? ""), g => g.ToList());
                    return Task.FromResult(Maya.Ext.Unit.Default);
                });
        }

        private Maya.Ext.Unit CreateDraft(WorklogDraft draftWorklog)
        {
            DraftWorklogs.Add((Model.UI.WorklogDraft)draftWorklog.Clone());
            return Maya.Ext.Unit.Default;
        }

        private async Task<Maya.Ext.Unit> CompleteDraftWorklog(WorklogDraft draftWorklog)
        {
            if (draftWorklog.StartTime == null || draftWorklog.EndTime == null)
            {
                await NotificationService.ShowError("Please set worklog times!");
                return Maya.Ext.Unit.Default;
            }

            if (string.IsNullOrEmpty(draftWorklog.IssueKey) || string.IsNullOrEmpty(draftWorklog.Title) || string.IsNullOrEmpty(draftWorklog.Description))
            {
                await NotificationService.ShowError("Please fill worklog data!");
                return Maya.Ext.Unit.Default;
            }

            var worklog = new Model.Db.Worklog
            {
                Description = draftWorklog.Description,
                EndTime = draftWorklog.EndTime.Value,
                IssueKey = draftWorklog.IssueKey,
                StartTime = draftWorklog.StartTime.Value,
                Title = draftWorklog.Title,
            };

            await Mediator.Send(new CQRS.Worklogs.Commands.CreateWorklogCommand(worklog))
                .HandleAsync(
                    async success =>
                    {
                        DraftWorklogs.Remove(draftWorklog);
                        await NotificationService.ShowSuccess("Successfully created worklog.");
                        await Load();
                    },
                    async fail =>
                    {
                        await NotificationService.ShowError($"Error occured on creating worklog. Message: {fail.Message}");
                    });

            return Maya.Ext.Unit.Default;
        }
    }
}
