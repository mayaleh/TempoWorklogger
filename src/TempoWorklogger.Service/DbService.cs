﻿using Polly;
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
        public async ValueTask<SQLiteAsyncConnection> GetConnection(bool updateSchema = false)
        {
            var conn = this.Connection;
            if (updateSchema)
            {
                await ApplyChangesOnShema(conn).ConfigureAwait(false);
            }
            return conn;
        }

        /// <inheritdoc/>
        public Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 10)
        {
            return Policy.Handle<SQLite.SQLiteException>()
                .WaitAndRetryAsync(numRetries, pollyRetryAttempt)
                .ExecuteAsync(action);

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
        }

        /// <summary>
        /// Generate the table in the database If the table already exists in the database checks the schema and attempts to update the database schema
        /// </summary>
        /// <param name="conn"></param>
        private static async Task ApplyChangesOnShema(SQLiteAsyncConnection conn)
        {
            await conn.EnableWriteAheadLoggingAsync().ConfigureAwait(false);

            await conn.CreateTableAsync<ColumnDefinition>().ConfigureAwait(false);
            await conn.CreateTableAsync<ImportMap>().ConfigureAwait(false);
        }
    }
}
