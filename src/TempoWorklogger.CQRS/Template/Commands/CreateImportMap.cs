using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.CQRS.Template.Commands
{
    public record CreateImportMapCommand(ImportMap ImportMap, ICollection<Model.Db.ColumnDefinition> Attributes) : IRequest<unitResult>;

    public class CreateImportMapCommandHandler : IRequestHandler<CreateImportMapCommand, unitResult>
    {
        private readonly IDbService dbService;

        public CreateImportMapCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(CreateImportMapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var importMap = request.ImportMap;

                if (string.IsNullOrWhiteSpace(importMap.Name))
                {
                    return unitResult.Failed(new Exception("Name is required!"));
                }

                var attributes = ColumnDefinitionHelper.MarkColumnsDefitionsAsAttributes(request.Attributes);

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var affectedRows = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.InsertAsync(importMap);
                }, cancellationToken).ConfigureAwait(false);

                if (importMap.ColumnDefinitions != null && importMap.ColumnDefinitions.Any())
                {
                    affectedRows += await CreateColumDefinitionsAsync(dbService, dbConnection, importMap.Id, importMap.ColumnDefinitions, cancellationToken);
                }

                if (attributes != null && attributes.Any())
                {
                    affectedRows += await CreateColumDefinitionsAsync(dbService, dbConnection, importMap.Id, attributes, cancellationToken);
                }

                return unitResult.Succeeded(Maya.Ext.Unit.Default);
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private static async Task<int> CreateColumDefinitionsAsync(
            IDbService dbService,
            SQLite.SQLiteAsyncConnection dbConnection,
            int importMapId,
            ICollection<Model.Db.ColumnDefinition> columnDefinitions,
            CancellationToken cancellationToken)
        {
            foreach (var columnDefinition in columnDefinitions)
            {
                columnDefinition.ImportMapId = importMapId;
            }

            return await dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
            {
                return dbConnection.InsertAllAsync(columnDefinitions);
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
