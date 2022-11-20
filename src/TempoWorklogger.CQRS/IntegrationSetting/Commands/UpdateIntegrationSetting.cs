namespace TempoWorklogger.CQRS.IntegrationSetting.Commands
{
    public record UpdateIntegrationSettingCommand(IntegrationSettings IntegrationSetting) : IRequest<unitResult>;

    public class UpdateIntegrationSettingHandler : IRequestHandler<UpdateIntegrationSettingCommand, unitResult>
    {
        private readonly IDbService dbService;

        public UpdateIntegrationSettingHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(UpdateIntegrationSettingCommand request, CancellationToken cancellationToken)
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

                var data = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.UpdateAsync(settings);
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
