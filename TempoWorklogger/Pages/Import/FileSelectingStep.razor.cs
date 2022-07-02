using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class FileSelectingStep
    {
        [Inject]
        public ImportState ImportState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public IBrowserFile selectedFile { get; set; }

        private void HandleFileSelected(InputFileChangeEventArgs callback)
        {
            selectedFile = callback.File;
        }

        private async Task OnNextClicked()
        {
            try
            {
                if (selectedFile != null)
                {
                    ImportState.File = new States.FileInfo
                    {
                        Name = selectedFile.Name,
                        ContentType = selectedFile.ContentType,
                        LastModified = selectedFile.LastModified,
                        Size = selectedFile.Size,
                    };

                    var stream = selectedFile.OpenReadStream();
                    await stream.CopyToAsync(ImportState.File.Content);
                    stream.Close();
                }

                if (ImportState.File != null)
                {
                    NavigationManager.NavigateTo("/import/select-map");
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
