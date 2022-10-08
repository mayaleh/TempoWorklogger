using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.Templates;
using TempoWorklogger.ViewModels.Templates;

namespace TempoWorklogger.Pages.Templates
{
    public partial class Templates : IDisposable
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        
        [Inject] 
        public IMediator Mediator { get; set; }
        
        private ITemplatesViewModel vm;

        protected override async Task OnInitializedAsync()
        {
            this.vm = new TemplatesViewModel(
                this.Mediator,
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
