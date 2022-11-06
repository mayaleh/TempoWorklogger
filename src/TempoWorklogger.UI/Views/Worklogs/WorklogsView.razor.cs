using Microsoft.AspNetCore.Components;
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

        async Task EditRow(Model.Db.Worklog worklog)
        {
            worklogToUpdate = worklog;
            await worklogsGrid!.EditRow(worklog);
        }

        async Task OnUpdateRow(Model.Db.Worklog worklog)
        {
            if (worklog == worklogToInsert)
            {
                worklogToInsert = null;
            }

            worklogToUpdate = null;

            await ViewModel.UpdateInlineCommand.Execute(worklog);

            // TODO: viewmodel.Commands.UpdateInlineCommand.Execute(worklog);
            //dbContext.Update(worklog);

            // For demo purposes only
            //worklog.Customer = dbContext.Customers.Find(worklog.CustomerID);
            //worklog.Employee = dbContext.Employees.Find(worklog.EmployeeID);

            // For production
            //dbContext.SaveChanges();
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

        //async Task DeleteRow(Model.Db.Worklog worklog)
        //{
        //    if (worklog == worklogToInsert)
        //    {
        //        worklogToInsert = null;
        //    }

        //    if (worklog == worklogToUpdate)
        //    {
        //        worklogToUpdate = null;
        //    }

        //    /*
        //    if (worklogs.Contains(worklog))
        //    {
        //        call delete command?
        //        // For demo purposes only
        //        worklogs.Remove(worklog);

        //        await worklogsGrid.Reload();
        //    }
        //    else
        //    {
        //        worklogsGrid.CancelEditRow(worklog);
        //        await worklogsGrid.Reload();
        //    }*/
        //}

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
            await ViewModel.CreateInlineCommand.Execute(worklog);
        }
    }
}
