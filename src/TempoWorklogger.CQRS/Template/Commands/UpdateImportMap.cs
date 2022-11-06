using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.CQRS.Template.Commands
{
    public record UpdateImportMapCommand(ImportMap ImportMap, ICollection<Model.Db.ColumnDefinition> Attributes) : IRequest<unitResult>;

    public class UpdateImportMapHandler : IRequestHandler<UpdateImportMapCommand, unitResult>
    {
        private readonly IDbService dbService;

        public UpdateImportMapHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(UpdateImportMapCommand request, CancellationToken cancellationToken)
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

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.UpdateAsync(importMap);
                }, cancellationToken).ConfigureAwait(false);

                return unitResult.Succeeded(Maya.Ext.Unit.Default);
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }
    }
}
