namespace TempoWorklogger.Contract.UI.ViewModels.Common
{
    public interface IProgressViewModel : IBaseViewModel, IDisposable
    {
        Action<int> OnProgressChanged { get; set; }
    }
}
