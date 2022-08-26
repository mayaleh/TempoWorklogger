using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.Library.Model;
using NPOI.SS.UserModel;

namespace TempoWorklogger.Library.Mapper.Tempo
{
    internal static class AttributeKeyValMapper
    {
        public static AttributeKeyVal MapFromColumnDefinitionStaticData(this AttributeKeyVal attributeKeyVal, ColumnDefinition columnDefinition)
        {
            attributeKeyVal.Key = columnDefinition.Name.Substring(nameof(AttributeKeyVal).Length);
            attributeKeyVal.Value = columnDefinition.Value;

            return attributeKeyVal;
        }
        public static AttributeKeyVal MapFromColumnDefinitionExcelCell(this AttributeKeyVal attributeKeyVal, ColumnDefinition columnDefinition, ICell cell)
        {
            attributeKeyVal.Key = columnDefinition.Name.Substring(nameof(AttributeKeyVal).Length);
            attributeKeyVal.Value = cell.ToString();

            return attributeKeyVal;
        }
    }
}
