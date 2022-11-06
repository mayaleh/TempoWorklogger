using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.Common;

namespace TempoWorklogger.UI.Commons
{
    public partial class ProgressBar
    {

        [CascadingParameter(Name = nameof(IProgressViewModel))]
        public IProgressViewModel ViewModel { get; set; } = null!;

        [Parameter]
        public string ProcessMessage { get; set; } = null!;

        private int progressState;

        protected override void OnInitialized()
        {
            ViewModel.OnProgressChanged = OnProgressChanged;
            base.OnInitialized();
        }

        private void OnProgressChanged(int percemtageDone)
        {
            this.progressState = percemtageDone;
            StateHasChanged();
        }
    }
}
