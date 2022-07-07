using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
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

        private void OnDeleteStorageClicked()
        {
            this.isReady = false;
            StorageService.ImportMapTemplate.DeleteStorage();
            this.LoadTempaltes();
            StateHasChanged();
        }
    }
}
