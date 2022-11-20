using Maya.Ext.Func.Rop;
using TempoWorklogger.Library;
using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record CreateWorklogCommand(Worklog Worklog, WorklogLogType creationType = WorklogLogType.None, DateTime? creationTimestamp = null) : IRequest<unitResult>;

    public class CreateWorklogCommandHandler : IRequestHandler<CreateWorklogCommand, unitResult>
    {
        private readonly IDbService dbService;

        public CreateWorklogCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(CreateWorklogCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.creationType == WorklogLogType.ImportFromFile && request.creationTimestamp == null)
                {
                    return unitResult.Failed(new ArgumentNullException(nameof(request.creationTimestamp), "On importing from file, the creation timespan is required."));
                }

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var worklog = request.Worklog;
                worklog.TimeSpentSeconds = WorklogHelper.CalculateTimeSpentSeconds(CommonConstants.Zero, worklog.StartTime, worklog.EndTime);

                var insertedRowCountResult = await this.dbService.ExecuteAttemptWithRetry((CancellationToken cancellationToken) =>
                    {
                        return dbConnection.InsertAsync(worklog);
                    }, cancellationToken)
                    .MapAsync(async affectedRow =>
                    {
                        affectedRow += affectedRow > 0 && worklog.Attributes.Any()
                            ? await CreateAttributesAsync(this.dbService, dbConnection, worklog.Id, worklog.Attributes, cancellationToken)
                            : 0;
                        return affectedRow;
                    })
                    .MapAsync(async affectedRows =>
                    {
                        affectedRows += request.creationType == WorklogLogType.ImportFromFile
                            ? await CreateInfoLogAsync(
                                dbService,
                                dbConnection,
                                worklog.Id,
                                WorklogLogType.ImportFromFile,
                                request.creationTimestamp,
                                "Imported successfully.",
                                cancellationToken)
                            : 0;
                        return affectedRows;
                    });

                return unitResult.Succeeded(Maya.Ext.Unit.Default);
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private static async Task<int> CreateAttributesAsync(
            IDbService dbService,
            SQLite.SQLiteAsyncConnection dbConnection,
            long worklogId,
            ICollection<Model.Db.CustomAttributeKeyVal> attributeKeyVals,
            CancellationToken cancellationToken)
        {
            foreach (var attributeKeyVal in attributeKeyVals)
            {
                attributeKeyVal.WorklogId = worklogId;
            }

            return await dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
            {
                return dbConnection.InsertAllAsync(attributeKeyVals);
            }, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<int> CreateInfoLogAsync(
            IDbService dbService,
            SQLite.SQLiteAsyncConnection dbConnection,
            long worklogId,
            WorklogLogType logType,
            DateTime? creationTimestamp,
            string message,
            CancellationToken cancellationToken)
        {
            return await dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
            {
                return dbConnection.InsertAsync(new WorklogLog
                {
                    WorklogId = worklogId,
                    Severity = LogSeverity.Information,
                    Type = logType,
                    Message = message,
                    Created = creationTimestamp ?? DateTime.Now,
                });
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
