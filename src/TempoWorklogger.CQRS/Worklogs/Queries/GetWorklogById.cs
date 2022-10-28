namespace TempoWorklogger.CQRS.Worklogs.Queries
{
    public record GetWorklogByIdQuery(long Id) : IRequest<worklogResult>;

    public class GetWorklogByIdQueryHander : IRequestHandler<GetWorklogByIdQuery, worklogResult>
    {
        private readonly IDbService dbService;

        public GetWorklogByIdQueryHander(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<worklogResult> Handle(GetWorklogByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var data = await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    return await dbConnection.Table<Worklog>()
                        .FirstOrDefaultAsync(w => w.Id == request.Id/*, cancellationToken*/);
                }, cancellationToken).ConfigureAwait(false);

                if (data == null)
                {
                    return worklogResult.Failed(new Exception($"Could not find worklog with id: {request.Id}"));
                }

                var attributes = await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    return await dbConnection.Table<CustomAttributeKeyVal>()
                        .Where(a => a.WorklogId == request.Id)
                        .ToListAsync(/*, cancellationToken*/);
                }, cancellationToken).ConfigureAwait(false);

                data.Attributes = attributes;

                return worklogResult.Succeeded(data);
            }
            catch (Exception e)
            {
                return worklogResult.Failed(e);
            }
        }
    }
}
