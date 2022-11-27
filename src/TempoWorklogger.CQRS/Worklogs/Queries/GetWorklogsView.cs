namespace TempoWorklogger.CQRS.Worklogs.Queries
{
    public record GetWorklogsViewQuery() : IRequest<worklogsViewResult>;

    public class GetWorklogsViewQueryHander : IRequestHandler<GetWorklogsViewQuery, worklogsViewResult>
    {
        private readonly IDbService dbService;

        public GetWorklogsViewQueryHander(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<worklogsViewResult> Handle(GetWorklogsViewQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = @"SELECT 
                                Worklog.*,
                                (SELECT 1 FROM WorklogLog WHERE WorklogLog.WorklogId = Worklog.Id AND WorklogLog.Type = 2 AND WorklogLog.Severity = 1 LIMIT 1) AS WasSendToTempo
                            FROM Worklog";

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.QueryAsync<WorklogView>(query);
                }, cancellationToken).ConfigureAwait(false);

                return worklogsViewResult.Succeeded(data ?? new List<WorklogView>());
            }
            catch (Exception e)
            {
                return worklogsViewResult.Failed(e);
            }
        }
    }
}
