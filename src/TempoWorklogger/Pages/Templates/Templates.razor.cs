using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
using System.Web;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Service;

namespace TempoWorklogger.Pages.Templates
{
    public partial class Templates
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IStorageService StorageService { get; set; }

        private string importMapsSource = string.Empty;

        private List<ImportMap> importMaps { get; set; }

        private string errorMessage = string.Empty;
        private bool isReady = false;

        private ImportMap importMapToDelete = new();

        protected override void OnInitialized()
        {
            StorageService.ImportMapTemplate.StorageSource()
                .Handle(
                    success => importMapsSource = success,
                    fail => importMapsSource = fail.Message
                );
            this.LoadTempaltes();
            base.OnInitialized();
        }

        private void LoadTempaltes()
        {
            StorageService.ImportMapTemplate.Read()
                .Handle(
                    success =>
                    {
                        this.importMaps = success;
                        isReady = true;
                    },
                    fail =>
                    {
                        errorMessage = fail.Message;
                        isReady = true;
                    }
                );
        }

        private void OnCreateClicked()
        {
            NavigationManager.NavigateTo("/templates/create");
        }
        private void OnEditClicked(string name)
        {
            var friendlyName = HttpUtility.UrlEncodeUnicode(name);
            NavigationManager.NavigateTo($"/templates/{friendlyName}/edit");
        }

        private void OnDeleteItemClicked(ImportMap item)
        {
            this.importMapToDelete = item;
        }

        private void OnDeleteItemConfirmClicked()
        {
            // TBD remove this.importMapToDelete from the storage and reset this.importMapToDelete and reload table
            StateHasChanged();
        }

        private void OnCancelDeleteItemClicked()
        {
            this.importMapToDelete = new();
            StateHasChanged();
        }
        

        private void OnDeleteStorageClicked()
        {
            this.isReady = false;
            StorageService.ImportMapTemplate.DeleteStorage();
            this.LoadTempaltes();
            StateHasChanged();
        }
    }
}
