using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels;
using TempoWorklogger.Contract.UI.ViewModels.IntegrationSettings;

namespace TempoWorklogger.UI.Views.IntegrationSettings
{
    public partial class IntegrationSettingsView : BaseInlineManagedGridView<Model.Db.IntegrationSettings>
    {
        [CascadingParameter(Name = nameof(IIntegrationSettingsViewModel))]
        public IIntegrationSettingsViewModel ViewModel { get; set; } = null!;

        protected override IBaseInlineManagedGridViewModel<Model.Db.IntegrationSettings> GridViewModel { get => ViewModel; }

        /*
        [CascadingParameter(Name = nameof(IIntegrationSettingsViewModel))]
        public IIntegrationSettingsViewModel ViewModel { get; set; } = null!;

        RadzenDataGrid<Model.Db.IntegrationSettings>? integrationSettingsGrid;

        Model.Db.IntegrationSettings? integrationSettingToInsert;
        Model.Db.IntegrationSettings? integrationSettingToUpdate;

        async Task EditRow(Model.Db.IntegrationSettings integrationSettings)
        {
            integrationSettingToUpdate = integrationSettings;
            await integrationSettingsGrid!.EditRow(integrationSettings);
        }

        async Task OnUpdateRow(Model.Db.IntegrationSettings integrationSettings)
        {
            integrationSettingToInsert = null;
            integrationSettingToUpdate = null;

            await ViewModel.UpdateInlineCommand.Execute(integrationSettings);
        }

        async Task SaveRow(Model.Db.IntegrationSettings integrationSettings)
        {
            await integrationSettingsGrid!.UpdateRow(integrationSettings);
        }

        void CancelEdit(Model.Db.IntegrationSettings integrationSettings)
        {
            if (integrationSettings == integrationSettingToInsert)
            {
                integrationSettingToInsert = null;
            }

            integrationSettingToUpdate = null;

            integrationSettingsGrid!.CancelEditRow(integrationSettings);
        }

        async Task InsertRow()
        {
            integrationSettingToInsert = new Model.Db.IntegrationSettings();
            await integrationSettingsGrid!.InsertRow(integrationSettingToInsert);
        }

        async Task OnCreateRow(Model.Db.IntegrationSettings integrationSettings)
        {
            integrationSettingToInsert = null;
            integrationSettingToUpdate = null;

            await ViewModel.CreateInlineCommand.Execute(integrationSettings);
        }*/
    }
}
