namespace TempoWorklogger.Contract.UI
{
    public interface ICommandAsync : IBaseCommand
    {
        Task Execute();
    }
}
