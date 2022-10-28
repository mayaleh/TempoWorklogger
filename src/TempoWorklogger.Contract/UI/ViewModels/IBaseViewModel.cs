using TempoWorklogger.Contract.UI.Core;

namespace TempoWorklogger.Contract.UI.ViewModels
{
    public interface IBaseViewModel
    {
        IUINotificationService NotificationService { get; }

        bool IsInit { get; set; }
        bool IsBusy { get; set; }

        Action<bool>? OnIsInit { get; set; }
        Action<bool>? OnIsBusy { get; set; }
    }
}
