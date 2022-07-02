using TempoWorklogger.Library.Model.Tempo;
using TempoWorklogger.Library.Model;

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
    }
}
