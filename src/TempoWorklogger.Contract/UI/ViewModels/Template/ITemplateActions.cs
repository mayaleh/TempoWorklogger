using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Contract.UI.ViewModels.Template
{
    public interface ITemplateActions
    {
        Task<Maya.Ext.Unit> Load(int? id);

        Task<Maya.Ext.Unit> Save();

        Task<Maya.Ext.Unit> AddAttribute();
        
        Task<Maya.Ext.Unit> RemoveAttribute(ColumnDefinition columnDefinition);
    }
}
