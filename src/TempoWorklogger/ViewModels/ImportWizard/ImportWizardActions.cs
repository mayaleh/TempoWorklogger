using Maya.Ext;
using Maya.Ext.Rop;
using Microsoft.AspNetCore.Components.Forms;
using TempoWorklogger.Contract.UI.ViewModels.ImportWizard;
using TempoWorklogger.CQRS.Template.Queries;
using TempoWorklogger.Model.UI;
using FileInfo = TempoWorklogger.Model.UI.FileInfo;

namespace TempoWorklogger.ViewModels.ImportWizard
{
    using stepExecutionResult = Result<ImportWizardStepKind, Exception>;

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

        public async Task<Maya.Ext.Unit> NextStep()
        {
            var stepExecutingAction = viewModel.ImportWizardState.CurrentStep switch
            {
                ImportWizardStepKind.File => SelectFileSetpAsync(),
                ImportWizardStepKind.Template => SelectImportTemplateStepAsync(),
                ImportWizardStepKind.Preview => ConfirmPreviewDataStepAsync(),
                ImportWizardStepKind.Process => ProcessImportStepAsync(),
                _ => throw new ApplicationException("There is no next step...")
            }; ;

            await stepExecutingAction.HandleAsync(
                success =>
                {
                    viewModel.ImportWizardState.CurrentStep = success;
                    return Task.CompletedTask;
                },
                fail =>
                {
                    viewModel.ErrorMessage = fail.Message;
                });

            return Maya.Ext.Unit.Default;
        }

        public Task<Maya.Ext.Unit> PreviousStep()
        {
            var currenctStepNr = (int)viewModel.ImportWizardState.CurrentStep - 1;

            if (Enum.IsDefined(typeof(ImportWizardStepKind), currenctStepNr) == false)
            {
                viewModel.GoBackToBase();
                return Task.FromResult(Maya.Ext.Unit.Default);
                //throw new ApplicationException("There is no previous step...");
            }

            viewModel.ImportWizardState.CurrentStep = (ImportWizardStepKind)currenctStepNr;

            return Task.FromResult(Maya.Ext.Unit.Default);
        }

        private async Task<stepExecutionResult> SelectFileSetpAsync()
        {
            try
            {
                if (viewModel.SelectedFile == null)
                {
                    return stepExecutionResult.Failed(new Exception("Please select valid file."));
                }

                UpdateFileInfoFromSelectedFile();

                var stream = viewModel.SelectedFile.OpenReadStream();
                await stream.CopyToAsync(viewModel.ImportWizardState.File.Content);
                stream.Close();

                if (viewModel.ImportWizardState.File == null)
                {
                    return stepExecutionResult.Failed(new Exception("Please select valid file."));
                }

                await LoadImportTemplates();

                return stepExecutionResult.Succeeded(ImportWizardStepKind.Template);
            }
            catch (Exception e)
            {
                return stepExecutionResult.Failed(e);
            }
        }

        private async Task LoadImportTemplates()
        {
            await viewModel.Mediator.Send(new GetImportMapsQuery())
                .HandleAsync(
                    success =>
                    {
                        viewModel.ImportMappingTemplates = success;
                        return Task.CompletedTask;
                    },
                    fail =>
                    {
                        viewModel.ErrorMessage = fail.Message;
                    }
                );
        }

        private async Task<stepExecutionResult> SelectImportTemplateStepAsync()
        {
            if (viewModel.ImportWizardState.File == null)
            {
                return stepExecutionResult.Failed(new Exception("Please select valid file."));
            }

            if (viewModel.ImportWizardState.SelectedImportMap == null)
            {
                return stepExecutionResult.Failed(new Exception("Please select valid mapping import template."));
            }

            // TODO: read the file

            return stepExecutionResult.Succeeded(ImportWizardStepKind.Preview);
        }

        private Task<stepExecutionResult> ConfirmPreviewDataStepAsync()
        {
            if (viewModel.ImportWizardState.WorklogsFileResults.Any(x => x.IsSuccess))
            {
                return Task.FromResult(stepExecutionResult.Succeeded(ImportWizardStepKind.Process));
            }

            return Task.FromResult(stepExecutionResult.Failed(new Exception("There is not any worklogs that could be imported from the file...")));
        }

        private async Task<stepExecutionResult> ProcessImportStepAsync()
        {
            throw new NotImplementedException();
            // return stepExecutionResult.Success(ImportWizardStepKind.Process) and the result
        }

        public void SelectedFileChanged(IBrowserFile selectedFile)
        {
            viewModel.SelectedFile = selectedFile;

            if (viewModel.SelectedFile == null)
            {
                viewModel.ImportWizardState.File = new();
                return;
            }
            UpdateFileInfoFromSelectedFile();
        }

        private void UpdateFileInfoFromSelectedFile()
        {
            viewModel.ImportWizardState.File = new FileInfo
            {
                Name = viewModel.SelectedFile.Name,
                ContentType = viewModel.SelectedFile.ContentType,
                LastModified = viewModel.SelectedFile.LastModified,
                Size = viewModel.SelectedFile.Size,
            };
        }

        public async Task<Unit> ReadFileContent()
        {
            try
            {
                await viewModel.Mediator.Send(new CQRS.Worklogs.Queries.ReadFromFileQuery(
                    viewModel.ImportWizardState.File,
                    viewModel.ImportWizardState.SelectedImportMap,
                    viewModel.OnProgressChanged))
                    .HandleAsync(
                    success =>
                    {
                        viewModel.ImportWizardState.WorklogsFileResults = success;
                        return Task.CompletedTask;
                    },
                    failure => {
                        viewModel.ErrorMessage = failure.Message;
                    });
            }
            catch (ObjectDisposedException e)
            {
                if (e.Message.Equals("Cannot access a closed Stream.", StringComparison.OrdinalIgnoreCase) == false)
                {
                    return Unit.Default;
                }

                viewModel.ErrorMessage = e.Message;
            }
            catch (Exception e)
            {
                viewModel.ErrorMessage = e.Message;
            }

            return Unit.Default;
        }
    }
}
