namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record CreateWorklogCommand(Worklog Worklog) : IRequest<unitResult>;

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
                var worklog = request.Worklog;

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var insertedRowCount = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.InsertAsync(worklog);
                }, cancellationToken).ConfigureAwait(false);

                if (insertedRowCount > 0 && worklog.Attributes.Any())
                {
                    insertedRowCount += await CreateAttributesAsync(this.dbService, dbConnection, worklog.Id, worklog.Attributes, cancellationToken);
                }

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
    }
}
