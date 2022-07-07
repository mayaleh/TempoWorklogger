using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoWorklogger.Library.Service;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class ExecutingProgressStep
    {
        [Inject]
        public ImportState ImportState { get; set; }

        [Inject]
        public ITempoService TempoService { get; set; }

        private bool isExecuting = true;

        private string errorMessage = string.Empty;

        private int processedProgress = 0;

        private int processedItemsCount = 0;

        private void OnStopImportClicked()
        {
            this.isExecuting = false;
        }

        private async Task OnContinueImportClicked()
        {
            this.isExecuting = true;
            await ExecuteImport();
        }

        private void OnReadProgressChanged(int percemtageDone)
        {
            this.processedProgress = percemtageDone;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await ExecuteImport();
            }
        }

        private async Task ExecuteImport()
        {
            // something like:
            // - allow stop and continue,
            // - handle results to display,
            // - progressbar,
            // - only 4 request pre second (maybe load as settings?)
            this.isExecuting = true;
            var itemNr = 0;
            foreach (var item in ImportState.WorklogsResults)
            {
                if (itemNr <= this.processedItemsCount)
                {
                    itemNr++;
                    continue;
                }
                
                await Task.Delay(6000/4); // only 4 requests per second

                if (this.isExecuting == false)
                {
                    break;
                }

                await TempoService.CreateWorklog(item.Success)
                    .HandleAsync(
                        success =>
                        {
                            itemNr++;
                            this.processedItemsCount = itemNr;
                            this.processedProgress = 100 * itemNr / ImportState.WorklogsResults.Count;
                            StateHasChanged();
                            // Todo sotre success
                            return Task.CompletedTask;
                        },
                        fail =>
                        {
                            // todo hanblde
                        }
                    );
            }
        }
    }
}
