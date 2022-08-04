using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.Library.Service;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class ImportMapSelectingStep
    {
        [Inject]
        public ImportState ImportState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IStorageService StorageService { get; set; }

        private string selectedMap
        {
            get => ImportState.ImportMap?.Name ?? string.Empty;
            set => ImportState.ImportMap = importMaps.FirstOrDefault(i => i.Name == value);
        }

        public IEnumerable<Library.Model.ColumnDefinition> ExcelCells
        {
            get => ImportState.ImportMap.ColumnDefinitions.Where(x => x.IsStatic == false)
                .OrderBy(x => x.Position);
        }

        public IEnumerable<Library.Model.ColumnDefinition> StaticValues
        {
            get => ImportState.ImportMap.ColumnDefinitions.Where(x => x.IsStatic);
        }

        private List<ImportMap> importMaps = new List<ImportMap>();

        private string errorMessage = string.Empty;
        private bool isReady = false;
        

        protected override void OnInitialized()
        {
            StorageService.ImportMapTemplate.Read()
                .Handle(
                    success =>
                    {
                        this.importMaps = success;
                        if (this.importMaps.Count == 1)
                        {
                            selectedMap = this.importMaps.First().Name;
                        }
                        isReady = true;
                    },
                    fail =>
                    {
                        errorMessage = fail.Message;
                        isReady = true;
                    }
                );

            base.OnInitialized();
        }

        private void OnNextClicked()
        {
            if (ImportState.File != null && ImportState.ImportMap != null)
            {
                NavigationManager.NavigateTo("/import/confirm-preview");
            }
        }
    }
}
