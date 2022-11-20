namespace TempoWorklogger.Contract.UI.Core
{
    public interface IUINotificationService
    {
        Task ShowError(string message);

        Task ShowSuccess(string message);
        
        Task ShowWarning(string message);

        Task ShowInfo(string message);
    }
}