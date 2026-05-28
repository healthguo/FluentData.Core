using System.Data;

namespace FluentData
{
    public partial class DbContext
    {
        /// <summary>
        /// Creates a new database command with the configured connection and transaction settings.
        /// Manages connection lifecycle based on transaction and shared connection flags.
        /// </summary>
        /// <returns>A new <see cref="DbCommand"/> instance ready for execution.</returns>
        private DbCommand CreateCommand
        {
            get
            {
                IDbConnection connection;
                if (Data.UseTransaction || Data.UseSharedConnection)
                {
                    if (Data.Connection == null)
                    {
                        Data.Connection = Data.AdoNetProvider.CreateConnection();
                        Data.Connection.ConnectionString = Data.ConnectionString;
                    }
                    connection = Data.Connection;
                }
                else
                {
                    connection = Data.AdoNetProvider.CreateConnection();
                    connection.ConnectionString = Data.ConnectionString;
                }
                var cmd = connection.CreateCommand();
                cmd.Connection = connection;

                return new DbCommand(this, cmd);
            }
        }

        /// <summary>
        /// Creates a new SQL command with the specified SQL text and optional parameters.
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <param name="parameters">Optional parameters for the SQL statement.</param>
        /// <returns>An <see cref="IDbCommand"/> instance for executing the query.</returns>
        public IDbCommand Sql(string sql, params object[] parameters)
        {
            var command = CreateCommand.Sql(sql).Parameters(parameters);
            return command;
        }

        /// <summary>
        /// Creates a command configured for multiple result set handling.
        /// Use this when executing queries that return multiple result sets.
        /// </summary>
        /// <returns>An <see cref="IDbCommand"/> instance configured for multiple results.</returns>
        public IDbCommand MultiResultSql
        {
            get
            {
                var command = CreateCommand.UseMultiResult(true);
                return command;
            }
        }
    }
}
