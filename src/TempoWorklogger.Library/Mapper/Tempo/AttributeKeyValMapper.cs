using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Library.Mapper.Tempo
{
    public static class AttributeKeyValMapper
    {
        public static AttributeKeyVal MapFromColumnDefinitionStaticData(this AttributeKeyVal attributeKeyVal, ColumnDefinition columnDefinition)
        {
            attributeKeyVal.Key = columnDefinition.Name.Substring(nameof(AttributeKeyVal).Length);
            attributeKeyVal.Value = columnDefinition.Value;

            return attributeKeyVal;
        }
    }
}
