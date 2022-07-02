using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoWorklogger.Pages.Import
{
    public partial class ExecutingProgressStep
    {
        private bool isDone = false;

        private string errorMessage = string.Empty;

        private int readProgress = 0;

        private void OnReadProgressChanged(int percemtageDone)
        {
            this.readProgress = percemtageDone;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                for (int i = 0; i < 100; i++)
                {
                    this.readProgress = i;
                    StateHasChanged();
                    await Task.Delay(500);
                }
            }
        }
    }
}
