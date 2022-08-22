using TempoWorklogger.Contract.UI;

namespace TempoWorklogger.UI.Core
{
    public abstract class BaseCommand : NotifyPropertyChanged, IBaseCommand
    {
        private bool canExecute;
        private bool executing;

        public bool CanExecute
        {
            get { return this.canExecute; }
            internal set
            {
                this.SetProperty(ref this.canExecute, value);
            }
        }

        public bool Executing
        {
            get { return this.executing; }
            internal set
            {
                this.SetProperty(ref this.executing, value);

                OnExecuteChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<bool>? OnExecuteChanged;
    }
}
