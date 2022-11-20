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
                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                
                var data = await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    var importMaps = await dbConnection.Table<ImportMap>()
                        .ToListAsync();
                    var importMapIds = importMaps.Select(x => x.Id);
                    
                    var columnDefinitions = await dbConnection.Table<ColumnDefinition>().Where(x => importMapIds.Contains(x.ImportMapId))
                        .ToListAsync();
                    
                    foreach (var importMap in importMaps) 
                    {
                        importMap.ColumnDefinitions = columnDefinitions.Where(x => x.ImportMapId == importMap.Id)
                            .ToList();
                    }

                    return importMaps;
                }, cancellationToken).ConfigureAwait(false);

                return importMapsResult.Succeeded(data ?? new List<ImportMap>());
            }
            catch (Exception e)
            {
                return importMapsResult.Failed(e);
            }
        }
    }
}
