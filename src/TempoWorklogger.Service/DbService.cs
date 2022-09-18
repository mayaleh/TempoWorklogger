using Polly;
using SQLite;
using TempoWorklogger.Contract.Services;
using TempoWorklogger.Model.Db;

namespace TempoWorklogger.Service
{
    public class DbService : IDbService
    {

        private readonly Lazy<SQLiteAsyncConnection> databaseConnectionHolder;
        private SQLiteAsyncConnection Connection => this.databaseConnectionHolder.Value;

        public DbService(string fileName)
        {
            this.databaseConnectionHolder = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(fileName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
        }

        //public async Task<SQLiteAsyncConnection> GetConnection(bool updateSchema = false)
        //var conn = new SQLiteAsyncConnection(fileName); // made lazy

        /// <inheritdoc/>
        public async ValueTask<SQLiteAsyncConnection> GetConnection(bool updateSchema = false, CancellationToken cancellationToken = default)
        {
            var conn = this.Connection;
            if (updateSchema)
            {
                await ApplyChangesOnShema(conn, cancellationToken).ConfigureAwait(false);
            }
            return conn;
        }

        /// <inheritdoc/>
        public Task<T> AttemptAndRetry<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken, int numRetries = 10)
        {
            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
            
            return Policy.Handle<SQLite.SQLiteException>()
                .WaitAndRetryAsync(numRetries, pollyRetryAttempt)
                .ExecuteAsync(action, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Generate the table in the database If the table already exists in the database checks the schema and attempts to update the database schema
        /// </summary>
        /// <param name="conn"></param>
        private static async Task ApplyChangesOnShema(SQLiteAsyncConnection conn, CancellationToken cancellationToken = default)
        {
            await conn.EnableWriteAheadLoggingAsync().ConfigureAwait(false);

            await conn.CreateTableAsync<ColumnDefinition>().ConfigureAwait(false);
            await conn.CreateTableAsync<ImportMap>().ConfigureAwait(false);
            await conn.CreateTableAsync<CustomAttributeKeyVal>().ConfigureAwait(false);
            await conn.CreateTableAsync<Worklog>().ConfigureAwait(false);
        }
    }
}
