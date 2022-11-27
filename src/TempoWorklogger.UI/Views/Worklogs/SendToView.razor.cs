using Microsoft.AspNetCore.Components;
using Radzen;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;

namespace TempoWorklogger.UI.Views.Worklogs
{
    public partial class SendToView
    {
        [CascadingParameter(Name = nameof(IWorklogsViewModel))]
        public IWorklogsViewModel ViewModel { get; set; } = null!;

        Model.Db.IntegrationSettings? selectedIntegrationSettings = null;

        bool isResultsTabSelected = false;

        void RowRender(RowRenderEventArgs<Model.Db.WorklogView> args)
        {
            if (args.Data.WasSendToTempo)
            {
                args.Attributes.Add("style", $"background-color: pink;");
            }
        }

        async Task ConfirmAndExecuteClicked()
        {
            isResultsTabSelected = true;
            await ViewModel.SendSelectedToApiCommand.Execute(selectedIntegrationSettings!);
        }
    }
}
