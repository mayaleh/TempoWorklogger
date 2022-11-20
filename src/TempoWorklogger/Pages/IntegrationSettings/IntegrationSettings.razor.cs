using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.IntegrationSettings;
using TempoWorklogger.ViewModels.IntegrationSettings;

namespace TempoWorklogger.Pages.IntegrationSettings
{
    public partial class IntegrationSettings
    {
        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IUINotificationService UINotificationService { get; set; }

        private IIntegrationSettingsViewModel vm;

        protected override async Task OnInitializedAsync()
        {
            this.vm = new IntegrationSettingsViewModel(
                this.Mediator,
                UINotificationService,
                () => StateHasChanged());

            await this.vm.LoadCommand.Execute();
        }

        public void Dispose()
        {
            this.vm.Dispose();
        }
    }
}
