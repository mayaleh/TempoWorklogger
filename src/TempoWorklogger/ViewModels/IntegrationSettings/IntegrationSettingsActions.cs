using Maya.Ext.Func.Rop;

namespace TempoWorklogger.ViewModels.IntegrationSettings
{
    using unitResult = Maya.Ext.Rop.Result<Maya.Ext.Unit, Exception>;

    internal class IntegrationSettingsActions
    {
        private readonly IntegrationSettingsViewModel vm;

        private Model.Db.IntegrationSettings integrationSettingsToBeDeleted = new();

        public IntegrationSettingsActions(IntegrationSettingsViewModel vm)
        {
            this.vm = vm;
        }

        public async Task<Maya.Ext.Unit> LoadIntegrationSettings()
        {
            var result = await this.vm.Mediator.Send(new CQRS.IntegrationSetting.Queries.GetIntegrationSettingsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
            }

            this.vm.IntegrationSettingsList = result.Success?.ToList() ?? new List<Model.Db.IntegrationSettings>();
            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> CreateInline(Model.Db.IntegrationSettings integrationSettings)
        {
            var result = await this.vm.Mediator.Send(new CQRS.IntegrationSetting.Commands.CreateIntegrationSettingCommand(integrationSettings));

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
                return Maya.Ext.Unit.Default;
            }

            this.vm.IntegrationSettingsList.Add(integrationSettings);

            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> UpdateInline(Model.Db.IntegrationSettings integrationSettings)
        {
            var result = await this.vm.Mediator.Send(new CQRS.IntegrationSetting.Commands.UpdateIntegrationSettingCommand(integrationSettings));

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
                return Maya.Ext.Unit.Default;
            }

            // maybe reload?

            return Maya.Ext.Unit.Default;
        }

        public Maya.Ext.Unit PrepareDeletion(Model.Db.IntegrationSettings toBeDeleted)
        {
            this.integrationSettingsToBeDeleted = toBeDeleted;
            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> Delete()
        {
            await this.vm.Mediator.Send(new CQRS.IntegrationSetting.Commands.DeleteIntegrationSettingCommand(integrationSettingsToBeDeleted))
                .EitherAsync(
                    async success =>
                    {
                        await this.vm.NotificationService.ShowSuccess($"Successfully deleted the settings.");
                        integrationSettingsToBeDeleted = new();
                        await LoadIntegrationSettings();
                        return unitResult.Succeeded(Maya.Ext.Unit.Default);
                    },
                    async failure =>
                    {
                        Console.WriteLine(failure.Message);
                        await this.vm.NotificationService.ShowError($"Failled to delete settings. Message: {failure.Message}");
                        return unitResult.Failed(failure);
                    }
                );
            return Maya.Ext.Unit.Default;
        }
    }
}
