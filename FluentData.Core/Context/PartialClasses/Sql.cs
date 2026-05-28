using System.Data;

namespace FluentData.Core
{
    public partial class DbContext
    {
        /// <summary>
        /// Creates a new <see cref="DbCommand"/> with the appropriate connection based on context settings.
        /// Uses shared connection when UseTransaction or UseSharedConnection is enabled.
        /// </summary>
        private DbCommand CreateCommand
        {
            get
            {
                IDbConnection? connection;
                if (Data.UseTransaction || Data.UseSharedConnection)
                {
                    if (Data.Connection == null)
                    {
                        Data.Connection = Data.AdoNetProvider.CreateConnection()!;
                        Data.Connection.ConnectionString = Data.ConnectionString;
                    }
                    connection = Data.Connection;
                }
                else
                {
                    connection = Data.AdoNetProvider.CreateConnection()!;
                    connection.ConnectionString = Data.ConnectionString;
                }
                var cmd = connection.CreateCommand();
                cmd.Connection = connection;

                return new DbCommand(this, cmd);
            }
        }

        /// <summary>
        /// Creates a new command with the specified SQL text and positional parameters.
        /// </summary>
        /// <param name="sql">The SQL text to execute.</param>
        /// <param name="parameters">The positional parameter values to add to the command.</param>
        /// <returns>A new <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand Sql(string sql, params object[] parameters)
        {
            var command = CreateCommand.Sql(sql).Parameters(parameters);
            return command;
        }

        /// <summary>
        /// Creates a new command configured to handle multiple result sets.
        /// </summary>
        /// <returns>A new <see cref="IDbCommand"/> instance with multi-result set handling enabled.</returns>
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
