using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;
using TempoWorklogger.Library.Extensions;

namespace TempoWorklogger.UI.Views.Worklogs
{
    public partial class WorklogsView
    {
        [CascadingParameter(Name = nameof(IWorklogsViewModel))]
        public IWorklogsViewModel ViewModel { get; set; } = null!;

        RadzenDataGrid<Model.Db.WorklogView>? worklogsGrid;

        Model.Db.WorklogView? worklogToInsert;
        Model.Db.WorklogView? worklogToUpdate;
        int rowsNumber = 25;
        List<Model.Db.Worklog> WorklogsBySelectedIssue = new();
        string worklogIssueKeyRowContext = string.Empty;
        //private DataGridSettings _settings = null!;

        //public DataGridSettings Settings
        //{
        //    get => _settings;
        //    set
        //    {
        //        _settings = value;
        //    }
        //}


        private string GetGridTotalTime()
        {
            if (worklogsGrid == null)
            {
                return string.Empty;
            }

            var totalMinutes = IntervalExtension.SummarizeIntervals(worklogsGrid!.View).TotalMilliseconds;
            return Convert.ToInt32((totalMinutes / 60)) + "h " + Convert.ToInt32((totalMinutes % 60)) + "m";
        }


        private string GetGridTotalTime(double totalMinutes)
        {
            return Convert.ToInt32((totalMinutes / 60)) + "h " + Convert.ToInt32((totalMinutes % 60)) + "m";
        }

        async Task RefreshGrid()
        {
            //Settings = worklogsGrid!.Settings;
            //var isActualSettingsNull = Settings == null || Settings!.Columns == null;
            //var actualFilters = isActualSettingsNull ? null : Settings!.Columns!.Select(x => new 
            //{ 
            //    x.Property,
            //    x.FilterValue,
            //    x.FilterOperator,
            //    x.SecondFilterValue,
            //    x.SecondFilterOperator,
            //    x.SortOrder,
            //    x.OrderIndex,
            //    x.Width
            //}).ToList();

            await ViewModel.LoadCommand.Execute();
            //if (actualFilters != null && isActualSettingsNull == false)
            //{
            //    foreach (var col in Settings!.Columns!)
            //    {
            //        var filters = actualFilters.FirstOrDefault(x => x.Property == col.Property);
            //        if (filters != null)
            //        {
            //            col.FilterValue = filters.FilterValue;
            //            col.FilterOperator = filters.FilterOperator;
            //            col.SecondFilterValue = filters.SecondFilterValue;
            //            col.SecondFilterOperator = filters.SecondFilterOperator;
            //            col.SortOrder = filters.SortOrder;
            //            col.OrderIndex = filters.OrderIndex;
            //            col.Width = filters.Width;
            //        }
            //    }
            //}
            await worklogsGrid!.Reload();
        }

        async Task EditRow(Model.Db.WorklogView worklog)
        {
            worklogToUpdate = worklog;
            worklogIssueKeyRowContext = worklog.IssueKey;
            await worklogsGrid!.EditRow(worklog);
        }

        async Task OnUpdateRow(Model.Db.WorklogView worklog)
        {
            worklogToInsert = null;
            worklogToUpdate = null;
            worklog.IssueKey = string.IsNullOrWhiteSpace(worklog.IssueKey) ? worklogIssueKeyRowContext : worklog.IssueKey;
            worklogIssueKeyRowContext = string.Empty;
            await ViewModel.UpdateInlineCommand.Execute(worklog);
        }

        async Task SaveRow(Model.Db.WorklogView worklog)
        {
            await worklogsGrid!.UpdateRow(worklog);
        }

        void CancelEdit(Model.Db.WorklogView worklog)
        {
            if (worklog == worklogToInsert)
            {
                worklogToInsert = null;
            }

            worklogToUpdate = null;
            worklogIssueKeyRowContext = string.Empty;

            worklogsGrid!.CancelEditRow(worklog);
        }

        async Task DuplicateRow(Model.Db.WorklogView worklog)
        {
            var duplicatesOf = (Model.Db.WorklogView)worklog.Clone();
            worklogIssueKeyRowContext = duplicatesOf.IssueKey!;
            await worklogsGrid!.InsertRow(duplicatesOf);
        }

        async Task InsertRow()
        {
            worklogIssueKeyRowContext = string.Empty;
            worklogToInsert = new Model.Db.WorklogView()
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
            };
            await worklogsGrid!.InsertRow(worklogToInsert);
        }

        async Task OnCreateRow(Model.Db.WorklogView worklog)
        {
            worklogToInsert = null;
            worklogToUpdate = null;
            worklog.IssueKey = string.IsNullOrWhiteSpace(worklog.IssueKey) ? worklogIssueKeyRowContext : worklog.IssueKey;
            worklogIssueKeyRowContext = string.Empty;
            await ViewModel.CreateInlineCommand.Execute(worklog);
        }

        void OnIssueKeyChanged(object args)
        {
            var text = args.ToString();
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            var selected = ViewModel.Worklogs.FirstOrDefault(x => x.IssueKey == text);

            if (selected == null)
            {
                return;
            }

            var autofill = worklogToInsert ?? worklogToUpdate;

            if (autofill != null)
            {
                autofill.Title = selected.Title;
                autofill.Description = selected.Description;
            }
        }


        void OnAutocompleteIssueKeyChanged(object args)
        {
            var issueKey = (string?)args;
            var currentWorklogRowContext = worklogToInsert ?? worklogToUpdate!;
            if (string.IsNullOrEmpty(issueKey) == false && ViewModel.AutoCompleteGroupdedWorklogs.TryGetValue(issueKey, out var worklogs))
            {
                currentWorklogRowContext.IssueKey = worklogs.First().IssueKey;
                WorklogsBySelectedIssue = worklogs;
                var firstW = worklogs.FirstOrDefault();
                currentWorklogRowContext.Title = firstW?.Title;
                currentWorklogRowContext.Description = firstW?.Description;
                return;
            }

            //currentWorklogRowContext.IssueKey = null;
            WorklogsBySelectedIssue = new();
        }
    }
}
