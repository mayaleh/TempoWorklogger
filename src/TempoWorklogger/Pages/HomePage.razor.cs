using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.HomePage;
using TempoWorklogger.ViewModels.HomePage;

namespace TempoWorklogger.Pages
{
    public partial class HomePage : IDisposable
    {
        [Inject]
        public IMediator Mediator { get; set; }

        [Inject]
        public IUINotificationService UINotificationService { get; set; }

        private IHomePageViewModel vm;

        protected override async Task OnInitializedAsync()
        {
            this.vm = new HomePageViewModel(
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
