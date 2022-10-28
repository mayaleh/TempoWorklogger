using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Worklog
{
    public interface IWorklogViewModel : IBaseViewModel, IDisposable
    {
        IWorklogCommands Commands { get; }

        IWorklogActions Actions { get; }

        Model.Db.Worklog WorklogModel { get; set; }

        void GoToEditWorklog(long worklogId);
    }

    public interface IWorklogActions
    {
        Task<Maya.Ext.Unit> Load(long? id);

        Task<Maya.Ext.Unit> Save();

        Task<Maya.Ext.Unit> AddAttribute(CustomAttributeKeyVal attributeKeyVal);

        Task<Maya.Ext.Unit> RemoveAttribute(CustomAttributeKeyVal attributeKeyVal);
    }

    public interface IWorklogCommands : IDisposable
    {
        /// <summary>
        /// Loads the Worklog by its ID, if null ID given, initialize default as new
        /// </summary>
        ICommandAsync<long?> LoadCommand { get; }

        /// <summary>
        /// Saves the Worklog model. Creates new one or updating the exists
        /// </summary>
        ICommandAsync SaveCommand { get; }

        /// <summary>
        /// Adds to Worklog an new record of the CustomAttributeKeyVal as custom attribute.
        /// </summary>
        ICommandAsync<CustomAttributeKeyVal> AddAttributeCommand { get; }

        /// <summary>
        /// Removes the CustomAttributeKeyVal that is a custom attribute.
        /// </summary>
        ICommandAsync<CustomAttributeKeyVal> RemoveAttributeCommand { get; }
    }
}
