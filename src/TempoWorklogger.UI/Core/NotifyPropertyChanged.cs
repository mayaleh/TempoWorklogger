using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TempoWorklogger.UI.Core
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// On change property notification.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingFiled"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        public void SetProperty<T>(ref T backingFiled, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;

            backingFiled = value;

            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notify INotifyPropertyChanged.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
