using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.UI;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    public class ImportWizardViewModel : BaseViewModel, IImportWizardViewModel
    {
        public ImportWizardState ImportWizardState { get; }

        public IImportWizardActions Actions { get; }
        
        public IImportWizardCommands Commands { get; }

        public IBrowserFile SelectedFile { get; set;  }

        public IEnumerable<ImportMap> ImportMappingTemplates { get; set; }
        
        public string ErrorMessage { get; set; }

        public Action<int> OnProgressChanged { get; set; }

        public ImportWizardViewModel(IMediator mediator, Action onUiChanged) : base(mediator, onUiChanged)
        {
            ImportWizardState = new();
            Actions = new ImportWizardActions(this);
            Commands = new ImportWizardCommands(this);
        }

        public void Dispose()
        {
            Commands.Dispose();
        }
    }
}
