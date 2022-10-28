using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Worklog;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Worklog
{
    public class WorklogViewModel : BaseViewModel, IWorklogViewModel
    {
        private readonly NavigationManager navigationManager;

        public WorklogViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            NavigationManager navigationManager,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            this.navigationManager = navigationManager;
            Actions = new WorklogActions(this);
            Commands = new WorklogCommands(this);
        }

        public IWorklogCommands Commands { get; }
        public IWorklogActions Actions { get; }
        public Model.Db.Worklog WorklogModel { get; set; }

        public void GoToEditWorklog(long worklogId)
        {
            this.navigationManager.NavigateTo($"/worklog/edit/{worklogId}", replace: true);
        }

        public void Dispose()
        {
            Commands?.Dispose();
        }
    }
}
