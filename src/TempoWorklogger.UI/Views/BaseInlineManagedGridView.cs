using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using TempoWorklogger.Contract.UI.ViewModels;

namespace TempoWorklogger.UI.Views
{
    public class BaseInlineManagedGridView<TModel> : ComponentBase
        where TModel : class, new()
    {
        protected virtual IBaseInlineManagedGridViewModel<TModel> GridViewModel { get; } = null!;

        protected RadzenDataGrid<TModel>? modelGrid;

        protected TModel? modelToInsert;
        protected TModel? modelToUpdate;

        protected async Task EditRow(TModel model)
        {
            modelToUpdate = model;
            await modelGrid!.EditRow(model);
        }

        protected async Task OnUpdateRow(TModel model)
        {
            modelToInsert = null;
            modelToUpdate = null;

            await GridViewModel.UpdateInlineCommand.Execute(model);
        }

        protected async Task SaveRow(TModel worklog)
        {
            await modelGrid!.UpdateRow(worklog);
        }

        protected void CancelEdit(TModel model)
        {
            if (model == modelToInsert)
            {
                modelToInsert = null;
            }

            modelToUpdate = null;

            modelGrid!.CancelEditRow(model);
        }

        protected async Task InsertRow()
        {
            modelToInsert = new TModel();
            await modelGrid!.InsertRow(modelToInsert);
        }

        protected async Task OnCreateRow(TModel model)
        {
            modelToInsert = null;
            modelToUpdate = null;

            await GridViewModel.CreateInlineCommand.Execute(model);
        }
    }
}
