namespace TempoWorklogger.CQRS.IntegrationSetting.Commands
{
    public record CreateIntegrationSettingCommand(IntegrationSettings IntegrationSetting) : IRequest<unitResult>;

    public class CreateIntegrationSettingCommandHandler : IRequestHandler<CreateIntegrationSettingCommand, unitResult>
    {
        private readonly IDbService dbService;

        public CreateIntegrationSettingCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(CreateIntegrationSettingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var settings = request.IntegrationSetting;

                if (string.IsNullOrWhiteSpace(settings.Name))
                {
                    return unitResult.Failed(new Exception("Name is required!"));
                }

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.InsertAsync(settings);
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
