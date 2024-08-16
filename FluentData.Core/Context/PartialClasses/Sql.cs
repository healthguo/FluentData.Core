using System.Data;

namespace FluentData.Core
{
    public partial class DbContext
    {
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

        public IDbCommand Sql(string sql, params object[] parameters)
        {
            var command = CreateCommand.Sql(sql).Parameters(parameters);
            return command;
        }

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
