using Maya.Ext.Rop;
using System.Collections;

namespace TempoWorklogger.CQRS.ColumnDefinitions.Queries
{
    using columnDefinitionsResult = Result<List<Model.Db.ColumnDefinition>, Exception>;

    public record GetAvailableColumnsQuery() : IRequest<columnDefinitionsResult>;

    public class GetAvailableColumnsQueryHandler : IRequestHandler<GetAvailableColumnsQuery, columnDefinitionsResult>
    {
        public Task<columnDefinitionsResult> Handle(GetAvailableColumnsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var columns = new List<Model.Db.ColumnDefinition>();
                var ignorePropertiesWithAttributes = new List<Type> { typeof(SQLite.PrimaryKeyAttribute), typeof(SQLite.AutoIncrementAttribute) };
                foreach (var property in typeof(Model.Db.Worklog).GetProperties())
                {
                    if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        continue;
                    }

                    // skip primary keys and autoincrements
                    if (property.CustomAttributes != null && property.CustomAttributes.Any(x => ignorePropertiesWithAttributes.Contains(x.AttributeType)))
                    {
                        continue;
                    }

                    columns.Add(new Model.Db.ColumnDefinition { Name = property.Name });
                }

                return Task.FromResult(columnDefinitionsResult.Succeeded(columns));
            }
            catch (Exception e)
            {
                return Task.FromResult(columnDefinitionsResult.Failed(e));
            }
        }
    }
}
