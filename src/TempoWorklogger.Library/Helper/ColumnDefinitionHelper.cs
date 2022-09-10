using TempoWorklogger.Model.Db;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Library.Helper
{
    public static class ColumnDefinitionHelper
    {
        /// <summary>
        /// Appends for the columns definitions an prefix for ability to identify it as an atribute. Appendig the prefix to Name
        /// </summary>
        /// <param name="columnDefinitions"></param>
        /// <returns></returns>
        public static ICollection<ColumnDefinition> MarkColumnsDefitionsAsAttributes(ICollection<ColumnDefinition> columnDefinitions)
        {
            if (columnDefinitions == null)
            {
                return columnDefinitions;
            }

            foreach (var item in columnDefinitions)
            {
                item.Name = nameof(AttributeKeyVal) + item.Name;
            }

            return columnDefinitions;
        }
    }
}
