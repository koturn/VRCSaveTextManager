using System;
using Microsoft.Data.Sqlite;
using VRCSaveTextManager.Resources;


namespace VRCSaveTextManager
{
    /// <summary>
    /// SQL Executor.
    /// </summary>
    public class BaseSqlExecutor : IDisposable
    {
        /// <summary>
        /// A flag property which indicates this instance is disposed or not.
        /// </summary>
        public bool IsDisposed { get; private set; } = false;
        /// <summary>
        /// Record editor name used for created_by and updated_by.
        /// </summary>
        public string RecordEditorName { get; set; }

        /// <summary>
        /// SQLite3 connection.
        /// </summary>
        private readonly SqliteConnection _connection;


        /// <summary>
        /// Connect database and create <see cref="BaseSqlExecutor"/> instance with it.
        /// </summary>
        /// <param name="connectionString">Connection string of SQLite database.</param>
        /// <param name="recordEditorName">Record editor name, used for "created_by" and "updated_by".</param>
        public BaseSqlExecutor(string connectionString, string recordEditorName = "app")
            : this(OpenDatabase(connectionString), recordEditorName)
        {
        }

        /// <summary>
        /// Create <see cref="BaseSqlExecutor"/> instance with specified connection.
        /// </summary>
        /// <param name="connection">Connection to SQLite database.</param>
        /// <param name="recordEditorName">Record editor name, used for "created_by" and "updated_by".</param>
        public BaseSqlExecutor(SqliteConnection connection, string recordEditorName = "app")
        {
            _connection = connection;
            RecordEditorName = recordEditorName;
        }

        /// <summary>
        /// Open SQLite database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns><see cref="SqliteConnection"/> instance.</returns>
        private static SqliteConnection OpenDatabase(string connectionString)
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = AppResource.GetText("config.sql");
            cmd.ExecuteNonQuery();

            return connection;
        }

        /// <summary>
        /// Begin transaction.
        /// </summary>
        /// <returns><see cref="SqliteTransaction"/> instance.</returns>
        public SqliteTransaction BeginTransaction()
        {
            return _connection.BeginTransaction();
        }

        /// <summary>
        /// Release all resources used by the <see cref="SqlExecutor"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Release all resources used by the <see cref="SqlExecutor"/> object.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Dispose();
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Execute "ANALYZE".
        /// </summary>
        public void Analyze()
        {
            ExecuteNoParamNonQuery("ANALYZE");
        }

        /// <summary>
        /// Execute "VACUUM".
        /// </summary>
        public void Vacuum()
        {
            ExecuteNoParamNonQuery("VACUUM");
        }

        /// <summary>
        /// Execute no parameter non query.
        /// </summary>
        /// <param name="sql">SQL text.</param>
        /// <returns>The number of rows inserted, updated, or deleted. -1 for SELECT statements.</returns>
        protected int ExecuteNoParamNonQuery(string sql)
        {
            using (var cmd = CreateCommand(sql))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Execute no parameter non query from specified resource.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>The number of rows inserted, updated, or deleted. -1 for SELECT statements.</returns>
        protected int ExecuteNoParamNonQueryFromResource(string resourceName)
        {
            using (var cmd = CreateCommandFromResource(resourceName))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create <see cref="SqliteCommand"/> from specified text resource and the connection, <see cref="_connection"/>.
        /// </summary>
        /// <param name="resourceName">Resource name.</param>
        /// <returns>Created <see cref="SqliteCommand"/> instance.</returns>
        protected SqliteCommand CreateCommandFromResource(string resourceName)
        {
            return CreateCommand(AppResource.GetText(resourceName));
        }

        /// <summary>
        /// Create <see cref="SqliteCommand"/> from specified SQL and the connection, <see cref="_connection"/>.
        /// </summary>
        /// <param name="sql">SQL text.</param>
        /// <returns>Created <see cref="SqliteCommand"/> instance.</returns>
        protected SqliteCommand CreateCommand(string sql)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = sql;
            return cmd;
        }

        /// <summary>
        /// Cast specified object to specified type.
        /// </summary>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <param name="obj">Target object.</param>
        /// <returns>Casted reference.</returns>
        protected static T? NullableCastRef<T>(object? obj)
            where T : class
        {
            return (obj == null || obj is DBNull) ? null : (T)obj;
        }

        /// <summary>
        /// Cast specified object to specified type.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="obj">Target object.</param>
        /// <returns>Casted <see cref="Nullable{T}"/> instance.</returns>
        protected static T? NullableCast<T>(object? obj)
            where T : struct
        {
            return (obj == null || obj is DBNull) ? null : (T)obj;
        }

        /// <summary>
        /// Enclose with single quotes.
        /// </summary>
        /// <param name="val">Target string</param>
        /// <returns>Enclosed <paramref name="val"/> with single quotes.</returns>
        protected static string SqlStringQuote(string val)
        {
            return "'" + SqlStringEscape(val) + "'";
        }

        /// <summary>
        /// Escape single quotes for SQL.
        /// </summary>
        /// <param name="val">Target string</param>
        /// <returns>Escaped <paramref name="val"/>.</returns>
        protected static string SqlStringEscape(string val)
        {
            return val.Replace("'", "''");
        }

        /// <summary>
        /// Convert <see cref="DateTime"/> to Unix timestamp, which is elapsed seconds since 1970-01-01 00:00:00.
        /// </summary>
        /// <param name="dt">A timestamp.</param>
        /// <returns>Unix timestamp.</returns>
        protected static long ToUnixTimestamp(DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeSeconds();
        }

        /// <summary>
        /// <para>Convert <see cref="DateTime"/> to Unix timestamp, which is elapsed seconds since 1970-01-01 00:00:00.</para>
        /// <para>If specified timestamp is null, returns <see cref="DBNull.Value"/>.</para>
        /// </summary>
        /// <param name="dt">A timestamp.</param>
        /// <returns>Unix timestamp.</returns>
        protected static object ToUnixTimestamp(DateTime? dt)
        {
            return dt.HasValue ? ToUnixTimestamp(dt.Value) : DBNull.Value;
        }

        /// <summary>
        /// Convert Unix timestamp to <see cref="DateTime"/>.
        /// </summary>
        /// <param name="unixTime">A unix timestamp.</param>
        /// <returns><see cref="DateTime"/> value.</returns>
        protected static DateTime FromUnixTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        /// <summary>
        /// <para>Convert Unix timestamp to <see cref="DateTime"/>.</para>
        /// <para>If specified timestamp is null, returns <see cref="DBNull.Value"/>.</para>
        /// </summary>
        /// <param name="unixTime">A unix timestamp.</param>
        /// <returns><see cref="DateTime"/> value.</returns>
        protected static DateTime? FromUnixTime(long? unixTime)
        {
            return unixTime.HasValue ? FromUnixTime(unixTime.Value) : null;
        }

        /// <summary>
        /// Returns <see cref="DBNull.Value"/> if specified values is null, otherwise as is.
        /// </summary>
        /// <typeparam name="T">Reference type.</typeparam>
        /// <param name="val">Target value.</param>
        /// <returns><see cref="DBNull.Value"/> if specified values is null, otherwise <paramref name="val"/>.</returns>
        protected static object CoalesceNullable<T>(T? val)
            where T : class
        {
            return val == null ? DBNull.Value : val;
        }

        /// <summary>
        /// Returns <see cref="DBNull.Value"/> if specified values is null, otherwise as is.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="val">Target value.</param>
        /// <returns><see cref="DBNull.Value"/> if specified values is null, otherwise <paramref name="val"/>.</returns>
        protected static object CoalesceNullable<T>(T? val)
            where T : struct
        {
            return val.HasValue ? val.Value : DBNull.Value;
        }

        /// <summary>
        /// Convert <see cref="bool"/> value to <see cref="int"/> value.
        /// </summary>
        /// <param name="b">A bool value.</param>
        /// <returns>Converted value.</returns>
        protected static int ToInt(bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// Convert <see cref="Nullable{T}"/> of <see cref="bool"/> value to <see cref="int"/> value or <see cref="DBNull.Value"/>.
        /// </summary>
        /// <param name="b">A bool value.</param>
        /// <returns>Converted value.</returns>
        protected  static object ToInt(bool? b)
        {
            return b.HasValue ? ToInt(b.Value) : DBNull.Value;
        }
    }
}
