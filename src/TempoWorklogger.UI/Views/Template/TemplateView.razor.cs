using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.Template;

namespace TempoWorklogger.UI.Views.Template
{
    public partial class TemplateView
    {
        [CascadingParameter(Name = nameof(ITemplateViewModel))]
        public ITemplateViewModel ViewModel { get; set; } = null!;

        string errorMessage = string.Empty;
    }
}
