using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoWorklogger.Library.Service;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class ConfirmWorklogsStep
    {
        [Inject]
        public IFileReaderService FileReaderService { get; set; }

        [Inject]
        public ImportState ImportState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private bool isReady = false;

        private string errorMessage = string.Empty;

        private int readProgress = 0;

        public ConfirmWorklogsStep()
        {
            // on load read the execl and show loading progress bar
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    if (ImportState.File == null || ImportState.File.Content == null || ImportState.File.Content.Length == 0)
                    {
                        NavigationManager.NavigateTo("/import/select-file");
                        return;
                    }

                    await ReadImportFile();
                }
                catch(ObjectDisposedException e)
                {
                    if (e.Message.Equals("Cannot access a closed Stream.", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        errorMessage = e.Message;
                    }
                }
                catch (Exception e)
                {
                    errorMessage = e.Message;
                }
                finally
                {
                    isReady = true;
                    StateHasChanged();
                }
            }

        }

        private async Task ReadImportFile()
        {
            ImportState.WorklogsResults = await FileReaderService.ReadWorklogFileAsync(ImportState.File.Content, ImportState.ImportMap, OnReadProgressChanged);
        }

        private void OnReadProgressChanged(int percemtageDone)
        {
            this.readProgress = percemtageDone;
            StateHasChanged();
        }

        private void OnConfirmClicked()
        {
            if (ImportState.WorklogsResults.Any(x => x.IsSuccess))
            {
                NavigationManager.NavigateTo("/import/processing");
            }
        }
    }
}
