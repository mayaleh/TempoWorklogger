using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using TempoWorklogger.Contract.UI.ViewModels.Worklog;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.UI.Views.Worklog
{
    public partial class WorklogView
    {
        [CascadingParameter(Name = nameof(IWorklogViewModel))]
        public IWorklogViewModel ViewModel { get; set; } = null!;

        RadzenDataGrid<CustomAttributeKeyVal>? customAttributesGrid;

        CustomAttributeKeyVal? customAttributeToInsert;
        CustomAttributeKeyVal? customAttributeToUpdate;

        async Task EditRow(CustomAttributeKeyVal customAttribute)
        {
            customAttributeToUpdate = customAttribute;
            await customAttributesGrid.EditRow(customAttribute);
        }

        void OnCreateRow(CustomAttributeKeyVal customAttribute)
        {
            // For demo purposes only

            // For production
            //dbContext.SaveChanges();
        }

        void OnUpdateRow(CustomAttributeKeyVal customAttribute)
        {
            if (customAttribute == customAttributeToInsert)
            {
                customAttributeToInsert = null;
            }

            customAttributeToUpdate = null;

            //dbContext.Update(customAttribute);
            //dbContext.SaveChanges();
        }

        async Task SaveRow(CustomAttributeKeyVal customAttribute)
        {
            customAttributeToUpdate = null;
            customAttributeToInsert = null;
            await customAttributesGrid.UpdateRow(customAttribute);
        }

        void CancelEdit(CustomAttributeKeyVal customAttribute)
        {
            if (customAttribute == customAttributeToInsert)
            {
                customAttributeToInsert = null;
            }

            customAttributeToUpdate = null;

            customAttributesGrid.CancelEditRow(customAttribute);

            // For production
            //var customAttributeEntry = dbContext.Entry(customAttribute);
            //if (customAttributeEntry.State == EntityState.Modified)
            //{
            //    customAttributeEntry.CurrentValues.SetValues(customAttributeEntry.OriginalValues);
            //    customAttributeEntry.State = EntityState.Unchanged;
            //}
        }

        async Task DeleteRow(CustomAttributeKeyVal customAttribute)
        {
            if (customAttribute == customAttributeToInsert)
            {
                customAttributeToInsert = null;
            }

            if (customAttribute == customAttributeToUpdate)
            {
                customAttributeToUpdate = null;
            }

            if (ViewModel.WorklogModel.Attributes.Contains(customAttribute))
            {
                await ViewModel.Commands.RemoveAttributeCommand.Execute(customAttribute);

                await customAttributesGrid.Reload();
                return;
            }

            customAttributesGrid.CancelEditRow(customAttribute);
            await customAttributesGrid.Reload();
        }


        async Task InsertRow()
        {
            customAttributeToInsert = new CustomAttributeKeyVal();
            await ViewModel.Commands.AddAttributeCommand.Execute(customAttributeToInsert);
            await customAttributesGrid.InsertRow(customAttributeToInsert);
        }
    }
}
