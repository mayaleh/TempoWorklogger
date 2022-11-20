namespace TempoWorklogger.CQRS.IntegrationSetting.Commands
{
    public record DeleteIntegrationSettingCommand(IntegrationSettings IntegrationSettings) : IRequest<unitResult>;

    public class DeleteCommandHandler : IRequestHandler<DeleteIntegrationSettingCommand, unitResult>
    {
        private readonly IDbService dbService;

        public DeleteCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(DeleteIntegrationSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var settings = request.IntegrationSettings;

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    return await dbConnection.Table<IntegrationSettings>()
                        .DeleteAsync(x => x.Id == settings.Id);
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
