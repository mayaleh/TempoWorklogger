namespace TempoWorklogger.CQRS.Template.Queries
{

    public record GetImportMapsQuery() : IRequest<importMapsResult>;

    public class GetImportMapsQueryHandler : IRequestHandler<GetImportMapsQuery, importMapsResult>
    {
        private readonly IDbService dbService;

        public GetImportMapsQueryHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<importMapsResult> Handle(GetImportMapsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbConnection = await this.dbService.GetConnection()
                    .ConfigureAwait(false);
                
                var data = await this.dbService.AttemptAndRetry(() =>
                {
                    return dbConnection.Table<ImportMap>()
                        .ToListAsync();
                }).ConfigureAwait(false);

                return importMapsResult.Succeeded(data ?? new List<ImportMap>());
            }
            catch (Exception e)
            {
                return importMapsResult.Failed(e);
            }
        }
    }
}
