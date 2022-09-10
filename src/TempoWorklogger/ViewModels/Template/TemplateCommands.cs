using TempoWorklogger.Contract.UI;
using TempoWorklogger.Contract.UI.ViewModels.Template;
using TempoWorklogger.UI.Core;

namespace TempoWorklogger.ViewModels.Template
{
    internal class TemplateCommands : ITemplateCommands
    {
        public TemplateCommands(ITemplateActions actions)
        {
            LoadCommand = new CommandAsync<int?>(actions.Load);
            SaveCommand = new CommandAsync(actions.Save);
            AddAttributeCommand = new CommandAsync(actions.AddAttribute);
            RemoveAttributeCommand = new CommandAsync<Model.Db.ColumnDefinition>(actions.RemoveAttribute);
        }
        /// <inheritdoc/>
        public ICommandAsync<int?> LoadCommand { get; }

        /// <inheritdoc/>
        public ICommandAsync SaveCommand { get; }

        /// <inheritdoc/>
        public ICommandAsync AddAttributeCommand { get; }

        /// <inheritdoc/>
        public ICommandAsync<Model.Db.ColumnDefinition> RemoveAttributeCommand { get; }
    }
}
