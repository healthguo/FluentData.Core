using System.Data;

namespace FluentData.Core
{
    internal partial class DbCommand : IDbCommand
    {
        public DbCommandData Data { get; private set; }

        public DbCommand(DbContext dbContext, System.Data.IDbCommand innerCommand)
        {
            Data = new DbCommandData(dbContext, innerCommand)
            {
                ExecuteQueryHandler = new ExecuteQueryHandler(this)
            };
        }

        public IDbCommand UseMultiResult(bool useMultipleResultset)
        {
            if (useMultipleResultset && !Data.Context.Data.FluentDataProvider.SupportsMultipleResultsets)
                throw new FluentDataException("The selected database does not support multiple resultset");

            Data.UseMultipleResultsets = useMultipleResultset;
            return this;
        }

        public IDbCommand CommandType(DbCommandTypes dbCommandType)
        {
            Data.InnerCommand.CommandType = (CommandType)dbCommandType;
            return this;
        }

        internal void ClosePrivateConnection()
        {
            if (!Data.Context.Data.UseTransaction && !Data.Context.Data.UseSharedConnection)
            {
                Data.InnerCommand.Connection?.Close();

                Data.Context.Data.OnConnectionClosed?.Invoke(new ConnectionEventArgs(Data.InnerCommand.Connection!));
            }
        }

        public void Dispose()
        {
            Data.Reader.Close();

            ClosePrivateConnection();
        }
    }
}
