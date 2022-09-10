using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Template
{
    /// <summary>
    /// Provides commands available for edit/add form of the ImportMap model also known as Import Template
    /// </summary>
    public interface ITemplateCommands
    {
        /// <summary>
        /// Loads the ImportMap by its ID, if null ID given, initialize default as new
        /// </summary>
        ICommandAsync<int?> LoadCommand { get; }

        /// <summary>
        /// Saves the ImportMap model. Creates new one or updating the exists
        /// </summary>
        ICommandAsync SaveCommand { get; }
        
        /// <summary>
        /// Adds to ImportMap an new record of the columnDefinition as custom attribute
        /// </summary>
        ICommandAsync AddAttributeCommand { get; }

        /// <summary>
        /// Removes the ColumnDefinition that is a custom attribute
        /// </summary>
        ICommandAsync<ColumnDefinition> RemoveAttributeCommand { get; }
    }
}
