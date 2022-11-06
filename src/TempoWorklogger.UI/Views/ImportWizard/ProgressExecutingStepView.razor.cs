using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;

namespace TempoWorklogger.UI.Views.ImportWizard
{
    public partial class ProgressExecutingStepView
    {
        [CascadingParameter(Name = nameof(IImportWizardViewModel))]
        public IImportWizardViewModel ViewModel { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel.Commands.ExecuteImportCommand.Execute();
            }
        }
    }
}
