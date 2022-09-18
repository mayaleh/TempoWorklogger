using TempoWorklogger.Model.Db;
using NPOI.SS.UserModel;

namespace TempoWorklogger.Library.Mapper.Db
{
    public static class CustomAttributeKeyValFromNPOI
    {
        public static CustomAttributeKeyVal MapFromColumnDefinitionStaticData(this CustomAttributeKeyVal attributeKeyVal, ColumnDefinition columnDefinition)
        {
            attributeKeyVal.Key = columnDefinition.Name[ModelsConstant.AttributePrefix.Length..];
            attributeKeyVal.Value = columnDefinition.Value;

            return attributeKeyVal;
        }

        public static CustomAttributeKeyVal MapFromColumnDefinitionExcelCell(this CustomAttributeKeyVal attributeKeyVal, ColumnDefinition columnDefinition, ICell cell)
        {
            attributeKeyVal.Key = columnDefinition.Name[ModelsConstant.AttributePrefix.Length..];
            attributeKeyVal.Value = cell.ToString();

            return attributeKeyVal;
        }
    }
}
