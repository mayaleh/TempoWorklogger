using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.Library.Service;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class ExecutingProgressStep
    {
        [Inject]
        public ImportState ImportState { get; set; }

        public ITempoService TempoService { get; set; }

        private bool isExecuting = true;
        private bool isDone = false;

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

        protected override void OnInitialized()
        {
            TempoService = new TempoService(ImportState.ImportMap.AccessToken);
            base.OnInitialized();
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
                if (itemNr < ImportState.WorklogResponseResults.Count)
                {
                    itemNr++;
                    continue;
                }

                if (item.IsFailure)
                {
                    itemNr++;
                    continue;
                }


                if (this.isExecuting == false)
                {
                    break;
                }

                var worklogRespnseResult = await TempoService.CreateWorklog(item.Success)
                    .EitherAsync(
                        success =>
                        {
                            var successResult = Result<(Worklog, WorklogResponse), (Worklog, Exception)>.Succeeded((item.Success, success));
                            return Task.FromResult(successResult);
                        },
                        failure =>
                        {
                            var failureResult = Result<(Worklog, WorklogResponse), (Worklog, Exception)>.Failed((item.Success, failure));
                            return Task.FromResult(failureResult);
                        }
                    );
                
                ImportState.WorklogResponseResults.Add(worklogRespnseResult);

                itemNr++;
                this.processedItemsCount = itemNr;
                this.processedProgress = 100 * itemNr / ImportState.WorklogsResults.Count;
                StateHasChanged();

                await Task.Delay(700);
            }

            this.isExecuting = false;
            this.isDone = true;
            StateHasChanged();
        }
    }
}
