using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;

namespace TempoWorklogger.UI.Views.ImportWizard
{
    public partial class SelectFileStepView
    {
        [CascadingParameter(Name = nameof(IImportWizardViewModel))]
        public IImportWizardViewModel ViewModel { get; set; } = null!;

        private void HandleFileSelected(InputFileChangeEventArgs callback)
        {
            ViewModel.Actions.SelectedFileChanged(callback.File);
        }
    }
}
