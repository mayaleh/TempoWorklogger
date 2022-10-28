using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Template
{
    public interface ITemplateViewModel : IBaseViewModel, IDisposable
    {
        ITemplateCommands Commands { get; }

        ITemplateActions Actions { get; }
        
        ImportMap ImportMapModel { get; set; }

        List<ColumnDefinition> AttributesModel { get; set; }
    }
}
