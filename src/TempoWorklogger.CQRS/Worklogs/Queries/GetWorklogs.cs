namespace TempoWorklogger.CQRS.Worklogs.Queries
{
    public record GetWorklogsQuery() : IRequest<worklogsResult>;

    public class GetWorklogsQueryHander : IRequestHandler<GetWorklogsQuery, worklogsResult>
    {
        private readonly IDbService dbService;

        public GetWorklogsQueryHander(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<worklogsResult> Handle(GetWorklogsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbConnection = await this.dbService.GetConnection()
                    .ConfigureAwait(false);

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.Table<Worklog>()
                        .ToListAsync();
                }, cancellationToken).ConfigureAwait(false);

                return worklogsResult.Succeeded(data ?? new List<Worklog>());
            }
            catch (Exception e)
            {
                return worklogsResult.Failed(e);
            }
        }
    }
}
