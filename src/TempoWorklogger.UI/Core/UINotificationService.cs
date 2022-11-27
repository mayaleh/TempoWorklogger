using Radzen;
using TempoWorklogger.Contract.UI.Core;

namespace TempoWorklogger.UI.Core
{
    public class UINotificationService : IUINotificationService
    {
        const int duration = 5000;
        const string style = "position:fixed;z-index:99999;top:100px;float:right;right:10px;";
        private readonly NotificationService notificationService;

        public UINotificationService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public Task ShowWarning(string message)
        {
            Console.WriteLine("{0} {1}", "Warning", message);
            notificationService.Notify(CreateMessage("Warning", message, NotificationSeverity.Warning));
            return Task.CompletedTask;
        }

        public Task ShowInfo(string message)
        {
            Console.WriteLine("{0} {1}", "Information", message);
            notificationService.Notify(CreateMessage("Information", message, NotificationSeverity.Info));
            return Task.CompletedTask;
        }

        public Task ShowError(string message)
        {
            Console.WriteLine("{0} {1}", "Error", message);
            notificationService.Notify(CreateMessage("Error occured", message, NotificationSeverity.Error));
            return Task.CompletedTask;
        }

        public Task ShowSuccess(string message)
        {
            Console.WriteLine("{0} {1}", "Success", message);
            notificationService.Notify(CreateMessage("Completed successfully", message, NotificationSeverity.Success));

            return Task.CompletedTask;
        }

        private static NotificationMessage CreateMessage(string title, string message, NotificationSeverity severity)
        {
            return new NotificationMessage
            {
                Duration = duration,
                Detail = message,
                Severity = severity,
                Summary = title
            };
        }
    }
}
