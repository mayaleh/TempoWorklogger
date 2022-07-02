using Microsoft.AspNetCore.Components;
using TempoWorklogger.Library.Model;
using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.States;

namespace TempoWorklogger.Pages.Import
{
    public partial class ImportMapSelectingStep
    {
        [Inject]
        public ImportState ImportState { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

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

        private List<ImportMap> importMaps = new List<ImportMap>(); // TODO load and if is single item, preselect.

        public ImportMapSelectingStep()
        {
            this.importMaps = new List<ImportMap>
            {
                new ImportMap
                {
                    Name = "Mock template",
                    StartFromRow = 2,
                    AccessToken = "",
                    ColumnDefinitions = new Library.Model.ColumnDefinition[]
                    {
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = false,
                            Position = "A",
                            Name = nameof(Worklog.IssueKey),
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = false,
                            Position = "B",
                            Name = nameof(Worklog.Description),
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = true,
                            Name = nameof(Worklog.AuthorAccountId),
                            Value = "XXIIIAAA"
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = true,
                            Name = nameof(AttributeKeyVal) + "_DODO_",
                            Value = "AtrrValue"
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = false,
                            Position = "C",
                            Name = nameof(Worklog.StartDate)
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = false,
                            Position = "D",
                            Name = nameof(Worklog.StartTime)
                        },
                        new Library.Model.ColumnDefinition()
                        {
                            IsStatic = false,
                            Position = "E",
                            Name = nameof(Worklog.EndTime)
                        },
                    }
                }
            };
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
