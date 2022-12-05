using Maya.Ext.Func.Rop;

namespace TempoWorklogger.ViewModels.Worklogs
{
    using unitResult = Maya.Ext.Rop.Result<Maya.Ext.Unit, Exception>;
    using tempoWorklogResponseResult = Maya.Ext.Rop.Result<Model.Tempo.WorklogResponse, System.Exception>;
    public class WorklogsActions
    {
        private readonly WorklogsViewModel vm;

        private Model.Db.WorklogView worklogToBeDeleted = new();

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
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Queries.GetWorklogsViewQuery());

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
            }

            this.vm.Worklogs = result.Success?.ToList() ?? new List<Model.Db.WorklogView>();

            var worklogsResult = await this.vm.Mediator.Send(new CQRS.Worklogs.Queries.GetWorklogsQuery());

            this.vm.AutoCompleteWorklogs = worklogsResult.IsSuccess && worklogsResult.Success != null ? worklogsResult.Success.ToList() : new List<Model.Db.Worklog>();
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

        public async Task<Maya.Ext.Unit> CreateInline(Model.Db.WorklogView worklog)
        {
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.CreateWorklogCommand((Model.Db.Worklog)worklog));

            if (result.IsFailure)
            {
                Console.WriteLine(result.Failure.Message);
                await this.vm.NotificationService.ShowError(result.Failure.Message);
                return Maya.Ext.Unit.Default;
            }

            this.vm.Worklogs.Add(worklog);

            await Load();

            return Maya.Ext.Unit.Default;
        }

        public async Task<Maya.Ext.Unit> UpdateInline(Model.Db.WorklogView worklog)
        {
            var result = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.UpdateWorklogCommand((Model.Db.Worklog)worklog, false));

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

        public Maya.Ext.Unit PrepareDeletion(Model.Db.WorklogView toBeDeleted)
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

                this.vm.SelectedWorklogs = new List<Model.Db.WorklogView>();

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

        public async Task DeleteOneWorklog(Model.Db.WorklogView worklog, bool doResetAndReload = true)
        {
            await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.DeleteCommand((Model.Db.Worklog)worklog))
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
                if (isSendToApiStopped && isSendingStoppedManualy && this.vm.SentToTempoResults.Count < this.vm.SelectedWorklogs.Count)
                {
                    await this.vm.NotificationService.ShowWarning("Continuing sending to Tempo API after manual stop is not allowed. Sorry, but refresh is required.");
                    return Maya.Ext.Unit.Default;
                }

                isSendToApiStopped = false;
                if (this.vm.SelectedWorklogs == null || !this.vm.SelectedWorklogs.Any())
                {
                    await this.vm.NotificationService.ShowError("Please, first select worklogs to send it to Tempo API...");
                    return Maya.Ext.Unit.Default;
                }

                var total = this.vm.SelectedWorklogs.Count;
                var itemNr = 0;

                this.vm.SentToTempoResults = new Dictionary<long, tempoWorklogResponseResult>(this.vm.SelectedWorklogs.Count);

                foreach (var item in this.vm.SelectedWorklogs)
                {
                    // TODO pause, stop, skip progressed
                    if (this.vm.SentToTempoResults.ContainsKey(item.Id))
                    {
                        continue; // Prevents sending already sent in this import seassion.
                    }

                    if (isSendToApiStopped)
                    {
                        await this.vm.NotificationService.ShowInfo("Stopped sending to Tempo API.");
                        break;
                    }

                    this.vm.SentToTempoResults[item.Id] = await this.vm.Mediator.Send(new CQRS.Worklogs.Commands.SendToTempoCommand(integrationSettings, item));

                    itemNr++;
                    this.vm.OnProgressChanged?.Invoke(itemNr * 100 / total);
                    this.vm.OnUiChanged.Invoke();
                }

                isSendToApiStopped = true;

                await Load();

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

        private bool isSendingStoppedManualy = false;

        public void StopSendingToTempo()
        {
            isSendToApiStopped = true;
            isSendingStoppedManualy = true;
        }

        public async Task<Maya.Ext.Unit> ResetSendToTempoData()
        {
            this.vm.SentToTempoResults = new Dictionary<long, tempoWorklogResponseResult>();

            if (this.vm.SentToTempoResults.Count > 0)
            {
                await LoadWorklogs();
            }

            return Maya.Ext.Unit.Default;
        }
    }
}
