using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.ViewModels.Worklogs;

namespace TempoWorklogger.UI.Views.Worklogs
{
    public partial class SendToView
    {
        [CascadingParameter(Name = nameof(IWorklogsViewModel))]
        public IWorklogsViewModel ViewModel { get; set; } = null!;

        Model.Db.IntegrationSettings? selectedIntegrationSettings = null;
    }
}
