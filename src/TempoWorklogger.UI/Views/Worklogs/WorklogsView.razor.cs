using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Text.Json;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;

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

        //void OnSelectRow(Model.Db.Worklog worklog)
        //{
        //    ViewModel.SelectedWorklogs.Add(worklog);
        //}

        //void OnDeselectRow(Model.Db.Worklog worklog)
        //{
        //    ViewModel.SelectedWorklogs.Remove(worklog);
        //}

        //@bind-Settings="@Settings"
        //DataGridSettings _settings;
        //public DataGridSettings Settings
        //{
        //    get
        //    {
        //        return _settings;
        //    }
        //    set
        //    {
        //        if (_settings != value)
        //        {
        //            _settings = value;
        //            InvokeAsync(SaveStateAsync);
        //        }
        //    }
        //}

        //string gridSettingState;

        //private Task SaveStateAsync()
        //{
        //    gridSettingState = JsonSerializer.Serialize<DataGridSettings>(Settings);
        //    return Task.CompletedTask;
        //}

        private string TotalGridTime => GetGridTotalTime();

        private string GetGridTotalTime()
        {
            if (worklogsGrid == null)
            {
                return string.Empty;
            }

            var totalMinutes = worklogsGrid!.View.Select(x => (x.EndTime - x.StartTime).TotalMinutes).Sum();
            return Convert.ToInt32((totalMinutes / 60)) + "h " + (totalMinutes % 60) + "m";
        }

        async Task RefreshGrid()
        {
            await ViewModel.LoadCommand.Execute();
            await worklogsGrid!.Reload();
            //Settings = JsonSerializer.Deserialize<DataGridSettings>(gridSettingState);
        }

        async Task EditRow(Model.Db.WorklogView worklog)
        {
            worklogToUpdate = worklog;
            await worklogsGrid!.EditRow(worklog);
        }

        async Task OnUpdateRow(Model.Db.WorklogView worklog)
        {
            worklogToInsert = null;
            worklogToUpdate = null;

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

            worklogsGrid!.CancelEditRow(worklog);
        }

        async Task DuplicateRow(Model.Db.WorklogView worklog)
        {
            var duplicatesOf = (Model.Db.WorklogView)worklog.Clone();

            await worklogsGrid!.InsertRow(duplicatesOf);
        }

        async Task InsertRow()
        {
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
    }
}
