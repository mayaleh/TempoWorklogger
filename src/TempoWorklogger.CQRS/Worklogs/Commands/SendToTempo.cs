using Maya.Ext.Func.Rop;
using TempoWorklogger.Library.Mapper.Tempo;

namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record SendToTempoCommand(Model.Db.IntegrationSettings IntegrationSettings, Worklog Worklog) : IRequest<unitResult>;

    public class SendToTempoCommandHandler : IRequestHandler<SendToTempoCommand, unitResult>
    {
        private readonly IDbService dbService;
        private readonly ITempoServiceFactory tempoServiceFactory;

        public SendToTempoCommandHandler(IDbService dbService, ITempoServiceFactory tempoServiceFactory)
        {
            this.dbService = dbService;
            this.tempoServiceFactory = tempoServiceFactory;
        }

        public async Task<unitResult> Handle(SendToTempoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tempoService = tempoServiceFactory.CreateService(request.IntegrationSettings);

                // Map DB.Worklog to Tempo.Worklog based on ImportMap and integration settings
                var result = await request.Worklog.MapWorklogDbToWorklogTempo(request.IntegrationSettings)
                    .BindAsync(tempoWorklog =>
                    {
                        return tempoService.WorklogService.Create(tempoWorklog); // send to Tempo API
                    })
                    .BindAsync(tempoResult =>
                    {
                        // Create log
                        return CreateSendLog(
                            request.Worklog.Id,
                            WorklogLogType.SendToTempoAttempt,
                            LogSeverity.Success,
                            "Successfully sent to Tempo API.",
                            cancellationToken);
                    })
                    .MapAsync(_ => Task.FromResult(Maya.Ext.Unit.Default));

                if (result.IsFailure)
                {
                    await CreateSendLog(
                        request.Worklog.Id,
                        WorklogLogType.SendToTempoAttempt,
                        LogSeverity.Error,
                        $"Failled to send to Tempo API. Message: {result.Failure.Message}",
                        cancellationToken);
                }

                return result;
            }
            catch (Exception e)
            {
                await CreateSendLog(
                    request.Worklog.Id,
                    WorklogLogType.SendToTempoAttempt,
                    LogSeverity.Error,
                    $"Failled to send to Tempo API. Message: {e.Message}",
                    cancellationToken);
                return unitResult.Failed(e);
            }
        }

        private async Task<Maya.Ext.Rop.Result<int, Exception>> CreateSendLog(
            long worklogId,
            WorklogLogType logType,
            LogSeverity logSeverity,
            string message,
            CancellationToken cancellationToken)
        {
            try
            {

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var result = await dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.InsertAsync(new WorklogLog
                    {
                        WorklogId = worklogId,
                        Severity = logSeverity,
                        Type = logType,
                        Message = message,
                        Created = DateTime.Now,
                    });
                }, cancellationToken).ConfigureAwait(false);

                return Maya.Ext.Rop.Result<int, Exception>.Succeeded(result);
            }
            catch (Exception e)
            {
                return Maya.Ext.Rop.Result<int, Exception>.Failed(e);
            }
        }
    }
}
