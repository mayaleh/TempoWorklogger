using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.CQRS.Template.Commands
{
    public record CreateImportMapCommand(ImportMap ImportMap, ICollection<ColumnDefinition> Attributes) : IRequest<unitResult>;

    public class CreateImportMapHandler : IRequestHandler<CreateImportMapCommand, unitResult>
    {
        private readonly IDbService dbService;

        public CreateImportMapHandler(IDbService dbService)
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

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.InsertAsync(importMap);
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
