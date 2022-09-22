using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.Model.UI;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    public class ImportWizardActions : IImportWizardActions
    {
        private readonly ImportWizardViewModel viewModel;

        public ImportWizardActions(ImportWizardViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public Task<Maya.Ext.Unit> CancelImport()
        {
            throw new NotImplementedException();
            return Task.FromResult(Maya.Ext.Unit.Default);
        }

        public Task<Maya.Ext.Unit> NextStep()
        {
            viewModel.ImportWizardState.CurrentStep = viewModel.ImportWizardState.CurrentStep switch
            {
                ImportWizardStepKind.File => ImportWizardStepKind.Template,
                ImportWizardStepKind.Template => ImportWizardStepKind.Preview,
                ImportWizardStepKind.Process => ImportWizardStepKind.Process,
                _ => throw new ApplicationException("There is no next step...")
            };

            return Task.FromResult(Maya.Ext.Unit.Default);
        }

        public Task<Maya.Ext.Unit> PreviousStep()
        {
            var currenctStepNr = (int)viewModel.ImportWizardState.CurrentStep - 1;

            if (Enum.IsDefined(typeof(ImportWizardStepKind), currenctStepNr) == false)
            {
                throw new ApplicationException("There is no previous step...");
            }

            viewModel.ImportWizardState.CurrentStep = (ImportWizardStepKind)currenctStepNr;

            return Task.FromResult(Maya.Ext.Unit.Default);
        }
    }
}
