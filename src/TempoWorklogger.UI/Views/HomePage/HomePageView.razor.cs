using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using TempoWorklogger.Contract.UI.ViewModels.HomePage;
using TempoWorklogger.Library.Extensions;
using TempoWorklogger.Model.UI;

namespace TempoWorklogger.UI.Views.HomePage
{
    public partial class HomePageView
    {
        [CascadingParameter(Name = nameof(IHomePageViewModel))]
        public IHomePageViewModel ViewModel { get; set; } = null!;

        RadzenDataGrid<Model.UI.WorklogDraft>? worklogsGrid;

        private Model.UI.WorklogDraft WorklogDraft { get; set; } = new Model.UI.WorklogDraft();

        private List<Model.Db.Worklog> WorklogsBySelectedIssue { get; set; } = new();

        private bool wasOriginalEndTimeEmpty = false;

        private string TotalDraftTime => GetTotalTime();

        private string issueKeyDraft = string.Empty;

        private string GetTotalTime()
        {
            var totalMinutes = IntervalExtension.SummarizeIntervals(ViewModel.DraftWorklogs.Where(x => x.StartTime != null && x.EndTime != null)).TotalMilliseconds;
            return Convert.ToInt32((totalMinutes / 60)) + "h " + (totalMinutes % 60) + "m";
        }

        private async Task CreateDraftWorkLog()
        {
            WorklogDraft.IssueKey ??= issueKeyDraft;
            ViewModel.CreateDraftCommand.Execute(WorklogDraft);

            if (ViewModel.DraftWorklogs.Any(x => x.IssueKey == WorklogDraft.IssueKey && x.Title == WorklogDraft.Title && x.StartTime == WorklogDraft.StartTime && x.Description == WorklogDraft.Description && x.EndTime == WorklogDraft.EndTime))
            {
                ResetDraftWorklog();
                await worklogsGrid!.Reload();
            }
        }

        private async Task CreateWorklogFromDraft()
        {
            await ViewModel.CompleteDraftWorklogCommand.Execute(WorklogDraft);
            if (ViewModel.DraftWorklogs.Contains(WorklogDraft) == false)
            {
                wasOriginalEndTimeEmpty = false;
                ResetDraftWorklog();
                await worklogsGrid!.Reload();
            }
        }

        private void PrepareCompleteDraft(WorklogDraft worklogDraft)
        {
            WorklogDraft = worklogDraft;
            wasOriginalEndTimeEmpty = worklogDraft.EndTime == null;
            worklogDraft.EndTime ??= new DateTime(DateTime.Now.Ticks);
        }

        private void ResetDraftWorklog()
        {
            WorklogDraft.EndTime = wasOriginalEndTimeEmpty ? null : WorklogDraft.EndTime;
            WorklogDraft = new();
            issueKeyDraft = string.Empty;
        }

        private void OnIssueKeyChanged(object value)
        {
            var issueKey = (string?)value;

            if (string.IsNullOrEmpty(issueKey) == false && ViewModel.Worklogs.TryGetValue(issueKey, out var worklogs))
            {
                WorklogDraft.IssueKey = worklogs.First().IssueKey;
                WorklogsBySelectedIssue = worklogs;
                var firstW = worklogs.FirstOrDefault();
                WorklogDraft.Title = firstW?.Title;
                WorklogDraft.Description = firstW?.Description;
                return;
            }

            WorklogDraft.IssueKey = null;
            WorklogsBySelectedIssue = new();
        }
    }
}
