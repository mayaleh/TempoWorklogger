using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Worklog;
using TempoWorklogger.ViewModels.Worklog;

namespace TempoWorklogger.Pages.Worklogs
{
    public partial class WorklogForm
    {
        [Parameter]
        public long? WorklogId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IUINotificationService UINotificationService { get; set; }

        private IWorklogViewModel vm;

        protected override async Task OnInitializedAsync()
        {
            this.vm = new WorklogViewModel(
                this.Mediator,
                UINotificationService,
                NavigationManager,
                () => StateHasChanged());

            await this.vm.Commands.LoadCommand.Execute(WorklogId);
        }

        public void Dispose()
        {
            this.vm.Dispose();
        }
    }
}
