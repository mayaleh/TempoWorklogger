using Microsoft.AspNetCore.Components.Forms;

namespace TempoWorklogger.Contract.UI.ViewModels.ImportWizard
{
    public interface IImportWizardActions
    {
        Task<Maya.Ext.Unit> NextStep();
        
        Task<Maya.Ext.Unit> PreviousStep();

        Task<Maya.Ext.Unit> CancelImport();

        Task<Maya.Ext.Unit> ReadFileContent();

        void SelectedFileChanged(IBrowserFile selectedFile);
        
        Task<Maya.Ext.Unit> ExecuteImportToDb();
    }
}
