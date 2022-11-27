﻿using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
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

        async Task RefreshGrid()
        {
            await ViewModel.LoadCommand.Execute();
            await worklogsGrid!.Reload();
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
