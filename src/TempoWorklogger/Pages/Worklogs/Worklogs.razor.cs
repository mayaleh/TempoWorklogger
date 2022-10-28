using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;
using TempoWorklogger.ViewModels.Worklogs;

namespace TempoWorklogger.Pages.Worklogs
{
    public partial class Worklogs : IDisposable
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IUINotificationService UINotificationService { get; set; }

        private IWorklogsViewModel vm;

        protected override async Task OnInitializedAsync()
        {
            this.vm = new WorklogsViewModel(
                this.Mediator,
                UINotificationService,
                NavigationManager,
                () => StateHasChanged());

            await this.vm.LoadCommand.Execute();
        }

        public void Dispose()
        {
            this.vm.Dispose();
        }
    }
}
