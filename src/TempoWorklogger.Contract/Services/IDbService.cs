using SQLite;

namespace TempoWorklogger.Contract.Services
{
    public interface IDbService
    {
        //Task<SQLiteAsyncConnection> GetConnection(bool updateSchema = false);
        /// <summary>
        /// Returns the connection to the DB
        /// </summary>
        /// <param name="updateSchema"></param>
        /// <returns></returns>
        ValueTask<SQLiteAsyncConnection> GetConnection(bool updateSchema = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// await AttemptAndRetry(() => GetConnection().Table<ImportMap>().ToListAsync()).ConfigureAwait(false);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="numRetries"></param>
        /// <returns></returns>
        Task<T> AttemptAndRetry<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken, int numRetries = 10);
    }
}
