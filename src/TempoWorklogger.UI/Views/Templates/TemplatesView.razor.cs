using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.Templates;

namespace TempoWorklogger.UI.Views.Templates
{
    public partial class TemplatesView
    {
        [CascadingParameter(Name = nameof(ITemplatesViewModel))]
        public ITemplatesViewModel ViewModel { get; set; } = null!;

        // TODO: move to base view model or toast notification center/manager
        private string errorMessage = string.Empty;
    }
}
