using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;

namespace TempoWorklogger.UI.Views.ImportWizard
{
    public partial class ImportWizardView
    {
        [CascadingParameter(Name = nameof(IImportWizardViewModel))]
        public IImportWizardViewModel ViewModel { get; set; } = null!;
    }
}
