namespace TempoWorklogger.CQRS.Template.Queries
{
    public record GetImportMapByIdQuery(int Id) : IRequest<importMapResult>;

    public class GetImportMapByIdQueryHandler : IRequestHandler<GetImportMapByIdQuery, importMapResult>
    {
        private readonly IDbService dbService;

        public GetImportMapByIdQueryHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<importMapResult> Handle(GetImportMapByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.Table<ImportMap>()
                        .FirstOrDefaultAsync(i => i.Id == request.Id);
                }, cancellationToken).ConfigureAwait(false);
                
                if (data == null)
                {
                    return importMapResult.Failed(new Exception($"The template with id {request.Id} not found..."));
                }

                return importMapResult.Succeeded(data);
            }
            catch (Exception e)
            {
                return importMapResult.Failed(e);
            }
        }
    }
}
