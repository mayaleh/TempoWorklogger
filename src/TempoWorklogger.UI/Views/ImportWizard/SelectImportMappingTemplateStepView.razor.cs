using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.UI.Views.ImportWizard
{
    public partial class SelectImportMappingTemplateStepView
    {
        [CascadingParameter(Name = nameof(IImportWizardViewModel))]
        public IImportWizardViewModel ViewModel { get; set; } = null!;

        private int selectedMap
        {
            get => ViewModel.ImportWizardState.SelectedImportMap?.Id ?? default(int);
            set => ViewModel.ImportWizardState.SelectedImportMap = ViewModel.ImportMappingTemplates.FirstOrDefault(i => i.Id == value) ?? new();
        }

        public IEnumerable<ColumnDefinition> ExcelCells
        {
            get => ViewModel.ImportWizardState.SelectedImportMap.ColumnDefinitions.Where(x => x.IsStatic == false)
                .OrderBy(x => x.Position);
        }

        public IEnumerable<ColumnDefinition> StaticValues
        {
            get => ViewModel.ImportWizardState.SelectedImportMap.ColumnDefinitions.Where(x => x.IsStatic);
        }
    }
}
