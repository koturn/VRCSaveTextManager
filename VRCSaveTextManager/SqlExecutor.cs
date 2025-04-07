using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


namespace VRCSaveTextManager
{
    /// <summary>
    /// SQL Executor.
    /// </summary>
    public sealed class SqlExecutor : BaseSqlExecutor
    {
        /// <summary>
        /// Connect database and create <see cref="SqlExecutor"/> instance with it.
        /// </summary>
        /// <param name="connectionString">Connection string of SQLite database.</param>
        /// <param name="recordEditorName">Record editor name, used for "created_by" and "updated_by".</param>
        public SqlExecutor(string connectionString, string recordEditorName = "app")
            : base(connectionString, recordEditorName)
        {
        }

        /// <summary>
        /// Create <see cref="SqlExecutor"/> instance with specified connection.
        /// </summary>
        /// <param name="connection">Connection to SQLite database.</param>
        /// <param name="recordEditorName">Record editor name, used for "created_by" and "updated_by".</param>
        public SqlExecutor(SqliteConnection connection, string recordEditorName = "app")
            : base(connection, recordEditorName)
        {
        }


        /// <summary>
        /// Execute DDL.
        /// </summary>
        public void InitializeDatabase()
        {
            ExecuteNoParamNonQueryFromResource("ddl.sql");
        }

        /// <summary>
        /// Create VIEW tables.
        /// </summary>
        public void CreateViews()
        {
            ExecuteNoParamNonQueryFromResource("create_view.sql");
        }

        /// <summary>
        /// SELECT log file names.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of log file names.</returns>
        public IEnumerable<string> SelectLogFile()
        {
            using (var cmd = CreateCommandFromResource("sel_t_logfile.sql"))
            using (var sdr = cmd.ExecuteReader())
            {
                while (sdr.Read())
                {
                    yield return sdr.GetString(0);
                }
            }
        }

        /// <summary>
        /// SELECT timestamp of specified log file.
        /// </summary>
        /// <param name="logFileName">Log file name.</param>
        /// <returns>Timestamp of specified log file.</returns>
        public IEnumerable<DateTime> SelectSaveTimestamp(string logFileName)
        {
            using (var cmd = CreateCommandFromResource("sel_t_save_text_log_at.sql"))
            {
                var p = cmd.Parameters;
                p.AddWithValue("@logfile", logFileName);

                using (var sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        yield return FromUnixTime(sdr.GetInt64(0));
                    }
                }
            }
        }

        /// <summary>
        /// SELECT one save data text.
        /// </summary>
        /// <param name="logAt">Log timestamp.</param>
        /// <returns>Save data text.</returns>
        public string? SelectOneSave(DateTime logAt)
        {
            using (var cmd = CreateCommandFromResource("sel_t_save_text_save.sql"))
            {
                var p = cmd.Parameters;
                p.AddWithValue("@log_at", ToUnixTimestamp(logAt));

                using (var sdr = cmd.ExecuteReader())
                {
                    return sdr.Read() ? sdr.GetString(0) : null;
                }
            }
        }

        /// <summary>
        /// INSERT ot UPDATE t_save_text.
        /// </summary>
        /// <param name="logAt">Save log timestamp.</param>
        /// <param name="saveText">Save data text.</param>
        /// <param name="logFrom">First timestamp of the log file.</param>
        /// <returns>The number of rows inserted or updated.</returns>
        public int UpsertSave(DateTime logAt, string saveText, DateTime logFrom)
        {
            using (var cmd = CreateCommandFromResource("ups_t_save_text_save.sql"))
            {
                var p = cmd.Parameters;
                p.AddWithValue("@log_at", ToUnixTimestamp(logAt));
                p.AddWithValue("@save_text", saveText);
                p.AddWithValue("@log_from", ToUnixTimestamp(logFrom));
                p.AddWithValue("@edit_by", RecordEditorName);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// UPDATE t_save_text.
        /// </summary>
        /// <param name="logFrom">First timestamp of the log file.</param>
        /// <param name="logUntil">Last timestamp of the log file..</param>
        /// <param name="logFileName">Log file name.</param>
        /// <returns>The number of rows updated.</returns>
        public int UpdateLogFile(DateTime logFrom, DateTime logUntil, string? logFileName)
        {
            using (var cmd = CreateCommandFromResource("upd_t_logfile.sql"))
            {
                var p = cmd.Parameters;
                p.AddWithValue("@log_from", ToUnixTimestamp(logFrom));
                p.AddWithValue("@log_until", ToUnixTimestamp(logUntil));
                p.AddWithValue("@logfile", logFileName);
                p.AddWithValue("@edit_by", RecordEditorName);
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// INSERT ot UPDATE t_save_text.
        /// </summary>
        /// <param name="logFrom">First timestamp of the log file.</param>
        /// <param name="logUntil">Last timestamp of the log file..</param>
        /// <param name="logFileName">Log file name.</param>
        /// <returns>The number of rows inserted or updated.</returns>
        public int UpsertLogFile(DateTime logFrom, DateTime logUntil, string? logFileName)
        {
            using (var cmd = CreateCommandFromResource("ups_t_logfile.sql"))
            {
                var p = cmd.Parameters;
                p.AddWithValue("@log_from", ToUnixTimestamp(logFrom));
                p.AddWithValue("@log_until", ToUnixTimestamp(logUntil));
                p.AddWithValue("@logfile", logFileName);
                p.AddWithValue("@edit_by", RecordEditorName);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
