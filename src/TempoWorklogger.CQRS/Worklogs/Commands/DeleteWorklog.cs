using SQLite;

namespace TempoWorklogger.CQRS.Worklogs.Commands
{
    public record DeleteCommand(Worklog Worklog, bool isForce = false) : IRequest<unitResult>;

    public class DeleteCommandHandler : IRequestHandler<DeleteCommand, unitResult>
    {
        private readonly IDbService dbService;

        public DeleteCommandHandler(IDbService dbService)
        {
            this.dbService = dbService;
        }

        public async Task<unitResult> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var worklog = request.Worklog;

                var dbConnection = await this.dbService.GetConnection(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                await this.dbService.AttemptAndRetry(async (CancellationToken cancellationToken) =>
                {
                    var affectedRows = 0;

                    if (request.isForce)
                    {
                        affectedRows += await DeleteWorklogRelationsAsync(dbConnection, worklog.Id);
                    }

                    affectedRows += await dbConnection.Table<Worklog>()
                        .DeleteAsync(x => x.Id == worklog.Id);
                    return affectedRows;
                }, cancellationToken).ConfigureAwait(false);

                return unitResult.Succeeded(Maya.Ext.Unit.Default);
            }
            catch (Exception e)
            {
                return unitResult.Failed(e);
            }
        }

        private static async Task<int> DeleteWorklogRelationsAsync(SQLiteAsyncConnection dbConnection, long worklogId)
        {
            var affectedRows = 0;

            affectedRows += await dbConnection.Table<CustomAttributeKeyVal>()
                .DeleteAsync(x => x.WorklogId == worklogId);

            //affectedRows += await dbConnection.Table<>()
            //    .DeleteAsync(x => x.WorklogId == worklogId);

            return affectedRows;
        }
    }
}
