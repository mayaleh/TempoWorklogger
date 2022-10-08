using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.ViewModels.ImportWizard;

namespace TempoWorklogger.Pages.Imports.Worklogs
{
    public partial class ImportWizard : IDisposable
    {
        [Inject]
        public IMediator Mediator { get; set; }

        private IImportWizardViewModel vm;

        protected override void OnInitialized()
        {
            this.vm = new ImportWizardViewModel(
                this.Mediator,
                () => StateHasChanged());

            base.OnInitialized();
        }

        public void Dispose()
        {
            this.vm.Dispose();
        }
    }
}
