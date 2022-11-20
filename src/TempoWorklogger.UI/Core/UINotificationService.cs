using Radzen;
using TempoWorklogger.Contract.UI.Core;

namespace TempoWorklogger.UI.Core
{
    public class UINotificationService : IUINotificationService
    {
        const int duration = 5000;
        private readonly NotificationService notificationService;

        public UINotificationService(NotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public Task ShowWarning(string message)
        {
            Console.WriteLine("{0} {1}", "Error", message);
            notificationService.Notify(
                new NotificationMessage
                {
                    Duration = duration,
                    Detail = message,
                    Severity = NotificationSeverity.Warning,
                    Summary = "Warning"
                });
            return Task.CompletedTask;
        }

        public Task ShowInfo(string message)
        {
            Console.WriteLine("{0} {1}", "Error", message);
            notificationService.Notify(
                new NotificationMessage
                {
                    Duration = duration,
                    Detail = message,
                    Severity = NotificationSeverity.Info,
                    Summary = "Information"
                });
            return Task.CompletedTask;
        }

        public Task ShowError(string message)
        {
            Console.WriteLine("{0} {1}", "Error", message);
            notificationService.Notify(
                new NotificationMessage
                {
                    Duration = duration,
                    Detail = message,
                    Severity = NotificationSeverity.Error,
                    Summary = "Error occured"
                });
            return Task.CompletedTask;
        }

        public Task ShowSuccess(string message)
        {
            Console.WriteLine("{0} {1}", "Success", message);
            notificationService.Notify(
                new NotificationMessage
                {
                    Duration = duration,
                    Detail = message,
                    Severity = NotificationSeverity.Success,
                    Summary = "Completed successfully"
                });

            return Task.CompletedTask;
        }
    }
}
