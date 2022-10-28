using MediatR;
using Microsoft.AspNetCore.Components;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels.Template;
using TempoWorklogger.Model.Db;
using TempoWorklogger.UI.Core;
using ColumnDefinition = TempoWorklogger.Model.Db.ColumnDefinition;

namespace TempoWorklogger.ViewModels.Template
{
    internal class TemplateViewModel : BaseViewModel, ITemplateViewModel
    {
        private readonly NavigationManager navigationManager;

        public TemplateViewModel(
            IMediator mediator,
            IUINotificationService notificationService,
            NavigationManager navigationManager,
            Action onUiChanged) : base(mediator, notificationService, onUiChanged)
        {
            this.navigationManager = navigationManager;
            Actions = new TemplateActions(this);
            Commands = new TemplateCommands(this);
        }

        public ITemplateCommands Commands { get; }

        public ITemplateActions Actions { get; }
        
        public ImportMap ImportMapModel { get; set; }

        public List<ColumnDefinition> AttributesModel { get; set; }

        public void Dispose()
        {
            Commands.Dispose();
        }
    }
}
