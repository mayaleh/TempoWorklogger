namespace TempoWorklogger.Contract.UI
{
    public interface ICommandAsync<TParam> : IBaseCommand
    {
        Task Execute(TParam input);
    }
}
