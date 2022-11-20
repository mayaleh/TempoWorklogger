namespace TempoWorklogger.Contract.UI.ViewModels
{
    public interface IBaseInlineManagedGridViewModel<TModel> : IBaseViewModel, IDisposable
        where TModel : class , new() 
    {
        List<TModel> IntegrationSettingsList { get; set; }

        ICommandAsync LoadCommand { get; }

        ICommandAsync DeleteCommand { get; }

        ICommand<TModel> PrepareDeleteCommand { get; }

        ICommandAsync<TModel> CreateInlineCommand { get; }

        ICommandAsync<TModel> UpdateInlineCommand { get; }
    }
}
