using TempoWorklogger.Library.Helper;
using TempoWorklogger.Library;

namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record UpdateWorklogCommand(Worklog Worklog, bool DoUpdateAttributes = true) : IRequest<unitResult>;

    public class UpdateWorklogCommandHandler : IRequestHandler<UpdateWorklogCommand, unitResult>
    {
        private readonly IDbService dbService;

        public UpdateWorklogCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(UpdateWorklogCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var worklog = request.Worklog;
                worklog.TimeSpentSeconds = WorklogHelper.CalculateTimeSpentSeconds(CommonConstants.Zero, worklog.StartTime, worklog.EndTime);

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                var updatedRowCount = await this.dbService.AttemptAndRetry((CancellationToken cancellationToken) =>
                {
                    return dbConnection.UpdateAsync(worklog);
                }, cancellationToken).ConfigureAwait(false);

                if (updatedRowCount > 0 && request.DoUpdateAttributes)
                {
                    updatedRowCount += await UpdateAttributesAsync(this.dbService, dbConnection, worklog.Id, worklog.Attributes, cancellationToken);
                }

                return unitResult.Succeeded(Maya.Ext.Unit.Default);
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private static async Task<int> UpdateAttributesAsync(
            IDbService dbService,
            SQLite.SQLiteAsyncConnection dbConnection,
            long worklogId,
            ICollection<Model.Db.CustomAttributeKeyVal> attributeKeyVals,
            CancellationToken cancellationToken)
        {
            return await dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
            {
                // TODO: transactional update
                var actualItemIds = attributeKeyVals.Where(x => x.Id != 0)
                .Select(x => x.Id)
                .ToList();

                var affectedRows = 0;

                // delete attributes, that is not present in new collection
                affectedRows += await dbConnection.Table<CustomAttributeKeyVal>()
                    .DeleteAsync(x => x.WorklogId == worklogId && actualItemIds.Contains(x.Id) == false);

                // replace existing to update and insert new ones
                foreach (var item in attributeKeyVals)
                {
                    item.WorklogId = worklogId;
                    affectedRows += await dbConnection.InsertOrReplaceAsync(attributeKeyVals);
                }

                return affectedRows;
            }, cancellationToken).ConfigureAwait(false);
        }
    }
}
