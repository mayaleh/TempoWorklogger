using Maya.Ext.Func.Rop;
using Maya.Ext.Rop;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.ViewModels.Worklogs
{
    using unitResult = Maya.Ext.Rop.Result<Maya.Ext.Unit, Exception>;
    public class WorklogsActions
    {
        private readonly WorklogsViewModel vm;

        private Model.Db.Worklog worklogToBeDeleted = new();

        private bool isSendToApiStopped = true;

        public WorklogsActions(WorklogsViewModel vm)
        {
            this.vm = vm;
        }

        public async Task<Maya.Ext.Unit> Load()
        {
            await LoadWorklogs();
            await LoadIntegrationSettings();

            return Maya.Ext.Unit.Default;
        }

        private async Task LoadWorklogs()
        {
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Queries.GetWorklogsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
            }

            this.vm.Worklogs = result.Success?.ToList() ?? new List<Model.Db.Worklog>();
        }

        private async Task LoadIntegrationSettings()
        {
            var result = await this.vm.Mediator.Send(new CQRS.IntegrationSetting.Queries.GetIntegrationSettingsQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
            }

            this.vm.IntegrationSettingsList = result.Success?.ToList() ?? new List<Model.Db.IntegrationSettings>();
        }

        public void CreateDetailed()
        {
            this.vm.NavigationManager.NavigateTo("/worklog/create");
        }

        public async Task<Maya.Ext.Unit> CreateInline(Model.Db.Worklog worklog)
        {
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.CreateWorklogCommand(worklog));

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
                return Maya.Ext.Unit.Default;
            }

            this.vm.Worklogs.Add(worklog);

            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> UpdateInline(Model.Db.Worklog worklog)
        {
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.UpdateWorklogCommand(worklog, false));

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
                return Maya.Ext.Unit.Default;
            }

            // maybe reload?

            return Maya.Ext.Unit.Default;
        }

        public Maya.Ext.Unit Edit(long id)
        {
            this.vm.NavigationManager.NavigateTo($"/worklog/edit/{id}");
            return Maya.Ext.Unit.Default;
        }

        public Maya.Ext.Unit PrepareDeletion(Model.Db.Worklog toBeDeleted)
        {
            this.worklogToBeDeleted = toBeDeleted;
            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> Delete()
        {
            if (this.vm.SelectedWorklogs != null && this.vm.SelectedWorklogs.Any())
            {
                foreach (var item in this.vm.SelectedWorklogs)
                {
                    await DeleteOneWorklog(item, false);
                }

                this.vm.SelectedWorklogs = new List<Model.Db.Worklog>();

                return await Load();
            }

            if (worklogToBeDeleted.Id == default(int))
            {
                await this.vm.NotificationService.ShowError("No worklog selected.");
                return Maya.Ext.Unit.Default;
            }

            await DeleteOneWorklog(this.worklogToBeDeleted);
            return Maya.Ext.Unit.Default;
        }

        public async Task DeleteOneWorklog(Model.Db.Worklog worklog, bool doResetAndReload = true)
        {
            await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.DeleteCommand(worklog))
                .EitherAsync(
                    async success =>
                    {
                        await this.vm.NotificationService.ShowSuccess($"Successfully deleted the worklog.");
                        if (doResetAndReload)
                        {
                            worklogToBeDeleted = new();
                            await Load();
                        }
                        return unitResult.Succeeded(Maya.Ext.Unit.Default);
                    },
                    async failure =>
                    {
                        Console.WriteLine(failure.Message);
                        await this.vm.NotificationService.ShowError($"Failled to delete th worklog. Message: {failure.Message}");
                        return unitResult.Failed(failure);
                    }
                );
        }

        public async Task<Maya.Ext.Unit> SendSelectedToTempoApi(Model.Db.IntegrationSettings integrationSettings)
        {
            try
            {
                isSendToApiStopped = false;
                if (this.vm.SelectedWorklogs == null || !this.vm.SelectedWorklogs.Any())
                {
                    await this.vm.NotificationService.ShowError("Please, first select worklogs to send it to Tempo API...");
                    return Maya.Ext.Unit.Default;
                }

                var total = this.vm.SelectedWorklogs.Count;
                var itemNr = 0;

                var importResults = new Dictionary<long, unitResult>(this.vm.SelectedWorklogs.Count); // TODO show results, move to VM state prop.

                foreach (var item in this.vm.SelectedWorklogs)
                {
                    // TODO pause, stop, skip progressed
                    if (importResults.ContainsKey(item.Id))
                    {
                        continue; // Prevents sending already sent in this import seassion.
                    }

                    if (isSendToApiStopped)
                    {
                        await this.vm.NotificationService.ShowInfo("Stopped sending to Tempo API.");
                        break;
                    }

                    importResults[item.Id] = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.SendToTempoCommand(integrationSettings, item));

                    itemNr++;
                    this.vm.OnProgressChanged?.Invoke(itemNr * 100 / total);
                }

                isSendToApiStopped = true;

                return Maya.Ext.Unit.Default;
            }
            catch (Exception e)
            {
                await this.vm.NotificationService.ShowError($"Error occured: {e.Message}...");
                return Maya.Ext.Unit.Default;
            }
            finally
            {
                isSendToApiStopped = true;
                await this.vm.NotificationService.ShowInfo("Sending to Tempo API completed.");
            }
        }
    }
}
