namespace TempoWorklogger.Contract.UI
{
    /// <summary>
    /// UI notify about its changes
    /// </summary>
    public interface INotifyUI
    {
        Action OnUiChanged { get; }
    }
}
