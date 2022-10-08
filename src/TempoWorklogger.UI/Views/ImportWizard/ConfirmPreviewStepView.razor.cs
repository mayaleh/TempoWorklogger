using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;

namespace TempoWorklogger.UI.Views.ImportWizard
{
    public partial class ConfirmPreviewStepView
    {
        [CascadingParameter(Name = nameof(IImportWizardViewModel))]
        public IImportWizardViewModel ViewModel { get; set; } = null!;

        // TODO move to somewhere or create action foor it in VM...
        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ViewModel.Commands.ReadFileContentCommand.Execute();
            }
        }

    }
}
