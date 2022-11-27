using Maya.Ext.Func.Rop;
using System.Text.Json;
using TempoWorklogger.Library.Mapper.Tempo;

namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    using tempoWorklogResponseResult = Maya.Ext.Rop.Result<Model.Tempo.WorklogResponse, System.Exception>;

    public record SendToTempoCommand(Model.Db.IntegrationSettings IntegrationSettings, Worklog Worklog) : IRequest<tempoWorklogResponseResult>;

    public class SendToTempoCommandHandler : IRequestHandler<SendToTempoCommand, tempoWorklogResponseResult>
    {
        private readonly IDbService dbService;
        private readonly ITempoServiceFactory tempoServiceFactory;

        public SendToTempoCommandHandler(IDbService dbService, ITempoServiceFactory tempoServiceFactory)
        {
            this.dbService = dbService;
            this.tempoServiceFactory = tempoServiceFactory;
        }

        public async Task<tempoWorklogResponseResult> Handle(SendToTempoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tempoService = tempoServiceFactory.CreateService(request.IntegrationSettings);

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
                            $"Successfully sent to Tempo API. Target: {request.IntegrationSettings.Name} - {request.IntegrationSettings.Endpoint}",
                            JsonSerializer.Serialize(new WorklogLogSendToTempoData(request.Worklog, request.IntegrationSettings.Endpoint, tempoResult)),
                            cancellationToken)
                        .MapAsync(_ => Task.FromResult(tempoResult));
                    });

                if (result.IsFailure)
                {
                    await CreateSendLog(
                        request.Worklog.Id,
                        WorklogLogType.SendToTempoAttempt,
                        LogSeverity.Error,
                        $"Failled to send to Tempo API. Message: {result.Failure.Message}; Target: {request.IntegrationSettings.Name} - {request.IntegrationSettings.Endpoint}",
                        JsonSerializer.Serialize(new WorklogLogSendToTempoData(request.Worklog, request.IntegrationSettings.Endpoint, null)),
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
                    $"Failled to send to Tempo API. Message: {e.Message}; Target: {request.IntegrationSettings.Name} - {request.IntegrationSettings.Endpoint}",
                    JsonSerializer.Serialize(new WorklogLogSendToTempoData(request.Worklog, request.IntegrationSettings.Endpoint, null)),
                    cancellationToken);
                return tempoWorklogResponseResult.Failed(e);
            }
        }

        private async Task<Maya.Ext.Rop.Result<int, Exception>> CreateSendLog(
            long worklogId,
            WorklogLogType logType,
            LogSeverity logSeverity,
            string message,
            string? data,
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
                        Data = data,
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
