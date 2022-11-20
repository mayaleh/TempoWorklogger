using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;

namespace TempoWorklogger.UI.Views.Worklogs
{
    public partial class WorklogsView
    {
        [CascadingParameter(Name = nameof(IWorklogsViewModel))]
        public IWorklogsViewModel ViewModel { get; set; } = null!;

        RadzenDataGrid<Model.Db.Worklog>? worklogsGrid;

        Model.Db.Worklog? worklogToInsert;
        Model.Db.Worklog? worklogToUpdate;
        int rowsNumber = 25;

        //void OnSelectRow(Model.Db.Worklog worklog)
        //{
        //    ViewModel.SelectedWorklogs.Add(worklog);
        //}

        //void OnDeselectRow(Model.Db.Worklog worklog)
        //{
        //    ViewModel.SelectedWorklogs.Remove(worklog);
        //}

        async Task EditRow(Model.Db.Worklog worklog)
        {
            worklogToUpdate = worklog;
            await worklogsGrid!.EditRow(worklog);
        }

        async Task OnUpdateRow(Model.Db.Worklog worklog)
        {
            worklogToInsert = null;
            worklogToUpdate = null;

            await ViewModel.UpdateInlineCommand.Execute(worklog);
        }

        async Task SaveRow(Model.Db.Worklog worklog)
        {
            await worklogsGrid!.UpdateRow(worklog);
        }

        void CancelEdit(Model.Db.Worklog worklog)
        {
            if (worklog == worklogToInsert)
            {
                worklogToInsert = null;
            }

            worklogToUpdate = null;

            worklogsGrid!.CancelEditRow(worklog);
        }

        async Task DuplicateRow(Model.Db.Worklog worklog)
        {
            var duplicatesOf = (Model.Db.Worklog)worklog.Clone();

            await worklogsGrid!.InsertRow(duplicatesOf);
        }

        async Task InsertRow()
        {
            worklogToInsert = new Model.Db.Worklog() 
            { 
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0),
                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0)
            };
            await worklogsGrid!.InsertRow(worklogToInsert);
        }

        async Task OnCreateRow(Model.Db.Worklog worklog)
        {
            worklogToInsert = null;
            worklogToUpdate = null;

            await ViewModel.CreateInlineCommand.Execute(worklog);
        }
    }
}
