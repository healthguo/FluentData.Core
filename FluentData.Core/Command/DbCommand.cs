using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Internal implementation of the database command interface.
    /// Provides the core functionality for executing SQL commands and stored procedures.
    /// </summary>
    internal partial class DbCommand : IDbCommand
    {
        /// <summary>
        /// Gets the command data containing SQL, reader, and execution handler.
        /// </summary>
        public DbCommandData Data { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DbCommand"/>.
        /// </summary>
        /// <param name="dbContext">The database context associated with this command.</param>
        /// <param name="innerCommand">The underlying ADO.NET <see cref="System.Data.IDbCommand"/>.</param>
        public DbCommand(DbContext dbContext, System.Data.IDbCommand innerCommand)
        {
            Data = new DbCommandData(dbContext, innerCommand)
            {
                ExecuteQueryHandler = new ExecuteQueryHandler(this)
            };
        }

        /// <summary>
        /// Configures whether the command should handle multiple result sets.
        /// </summary>
        /// <param name="useMultipleResultset">True to enable multiple result sets; otherwise, false.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        /// <exception cref="FluentDataException">Thrown if the selected database does not support multiple result sets.</exception>
        public IDbCommand UseMultiResult(bool useMultipleResultset)
        {
            if (useMultipleResultset && !Data.Context.Data.FluentDataProvider.SupportsMultipleResultsets)
                throw new FluentDataException("The selected database does not support multiple resultset");

            Data.UseMultipleResultsets = useMultipleResultset;
            return this;
        }

        /// <summary>
        /// Sets the command type (Text or StoredProcedure).
        /// </summary>
        /// <param name="dbCommandType">The type of command to execute.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        public IDbCommand CommandType(DbCommandTypes dbCommandType)
        {
            Data.InnerCommand.CommandType = (CommandType)dbCommandType;
            return this;
        }

        /// <summary>
        /// Closes the private connection if not using a transaction or shared connection.
        /// </summary>
        internal void ClosePrivateConnection()
        {
            if (!Data.Context.Data.UseTransaction && !Data.Context.Data.UseSharedConnection)
            {
                Data.InnerCommand.Connection?.Close();

                Data.Context.Data.OnConnectionClosed?.Invoke(new ConnectionEventArgs(Data.InnerCommand.Connection!));
            }
        }

        /// <summary>
        /// Disposes the command, closing the data reader and connection if necessary.
        /// </summary>
        public void Dispose()
        {
            Data.Reader.Close();

            ClosePrivateConnection();
        }
    }
}
