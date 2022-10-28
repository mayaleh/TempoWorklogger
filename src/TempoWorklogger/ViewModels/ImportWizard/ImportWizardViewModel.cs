using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.UI;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    public class ImportWizardViewModel : BaseViewModel, IImportWizardViewModel
    {
        private readonly NavigationManager navigationManager;

        public ImportWizardState ImportWizardState { get; }

        public IImportWizardActions Actions { get; }

        public IImportWizardCommands Commands { get; }

        public IBrowserFile SelectedFile { get; set; }

        public IEnumerable<ImportMap> ImportMappingTemplates { get; set; }

        public string ErrorMessage { get; set; }

        public Action<int> OnProgressChanged { get; set; }

        public ImportWizardViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            NavigationManager navigationManager,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            ImportWizardState = new();
            Actions = new ImportWizardActions(this);
            Commands = new ImportWizardCommands(this);
            this.navigationManager = navigationManager;
        }

        public void Dispose()
        {
            Commands.Dispose();
        }

        public void GoBackToBase()
        {
            this.navigationManager.NavigateTo("/");
        }
    }
}
