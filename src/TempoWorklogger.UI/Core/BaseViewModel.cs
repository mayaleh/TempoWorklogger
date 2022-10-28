using MediatR;
using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.Core;
using TempoWorklogger.Contract.UI.ViewModels;

namespace TempoWorklogger.UI.Core
{
    public class BaseViewModel : NotifyPropertyChanged, INotifyUI, IBaseViewModel
    {
        bool isInit, isBusy;

        public IMediator Mediator { get; }

        public IUINotificationService NotificationService { get; }

        public Action OnUiChanged { get; }

        public Action<bool>? OnIsInit { get; set; }

        public Action<bool>? OnIsBusy { get; set; }

        public bool IsInit
        {
            get { return isInit; }
            set
            {
                this.isInit = value;

                OnIsInit?.Invoke(value);
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                this.isBusy = value;

                OnIsBusy?.Invoke(value);
            }
        }

        public BaseViewModel(IMediator mediator, IUINotificationService notificationService, Action onUiChanged)
        {
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            NotificationService = notificationService;
            this.OnUiChanged = onUiChanged ?? throw new ArgumentNullException(nameof(onUiChanged));

            this.OnIsInit = (v) => { this.OnUiChanged.Invoke(); };
            this.OnIsBusy = (v) => { this.OnUiChanged.Invoke(); };
        }

    }
}
