namespace TempoWorklogger.CQRS.IntegrationSetting.Queries
{
    public record GetIntegrationSettingsQuery() : IRequest<integrationSettingsResult>;

    public class GetIntegrationSettingsQueryHandler : IRequestHandler<GetIntegrationSettingsQuery, integrationSettingsResult>
    {
        private readonly IDbService dbService;

        public GetIntegrationSettingsQueryHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<integrationSettingsResult> Handle(GetIntegrationSettingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var data = await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    return await dbConnection.Table<Model.Db.IntegrationSettings>()
                        .ToListAsync();
                }, cancellationToken).ConfigureAwait(false);

                return integrationSettingsResult.Succeeded(data ?? new List<Model.Db.IntegrationSettings>());
            }
            catch (Exception e)
            {
                return integrationSettingsResult.Failed(e);
            }
        }
    }
}
