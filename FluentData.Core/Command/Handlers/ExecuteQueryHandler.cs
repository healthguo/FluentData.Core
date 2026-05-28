using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Handles query execution lifecycle including connection management, transaction handling, and error processing.
    /// Manages the preparation, execution, and cleanup of database commands.
    /// </summary>
    internal class ExecuteQueryHandler
    {
        private readonly DbCommand _command;

        private bool _queryAlreadyExecuted;

        /// <summary>
        /// Creates a new instance of <see cref="ExecuteQueryHandler"/>.
        /// </summary>
        /// <param name="command">The database command to handle execution for.</param>
        public ExecuteQueryHandler(DbCommand command)
        {
            _command = command;
        }

        /// <summary>
        /// Executes the query with proper lifecycle management including preparation, execution, and cleanup.
        /// </summary>
        /// <param name="useReader">Whether to use a data reader for results.</param>
        /// <param name="action">The action to execute for query processing.</param>
        /// <param name="isNextResult">Whether to move to the next result set (for multiple result sets).</param>
        /// <exception cref="FluentDataException">Thrown if a query has already been executed on this command without multiple result set support.</exception>
        internal void ExecuteQuery(bool useReader, Action action, bool isNextResult = true)
        {
            try
            {
                PrepareDbCommand(useReader, isNextResult);
                action();

                _command.Data.Context.Data.OnExecuted?.Invoke(new CommandEventArgs(_command.Data.InnerCommand));
            }
            catch (Exception exception)
            {
                HandleQueryException(exception);
            }
            finally
            {
                HandleQueryFinally();
            }
        }

        /// <summary>
        /// Prepares the database command for execution by setting SQL, connection, and transaction.
        /// </summary>
        /// <param name="useReader">Whether to use a data reader for results.</param>
        /// <param name="isNextResult">Whether to move to the next result set.</param>
        private void PrepareDbCommand(bool useReader, bool isNextResult)
        {
            if (_queryAlreadyExecuted)
            {
                if (_command.Data.UseMultipleResultsets)
                {
                    if (isNextResult)
                    {
                        _command.Data.Reader.NextResult();
                    }
                }
                else
                {
                    throw new FluentDataException("A query has already been executed on this command object. Please create a new command object.");
                }
            }
            else
            {
                var innerCommand = _command.Data.InnerCommand;
                innerCommand.CommandText = _command.Data.Sql.ToString();

                var contextData = _command.Data.Context.Data;
                if (contextData.CommandTimeout != int.MinValue)
                    innerCommand.CommandTimeout = contextData.CommandTimeout;

                if (innerCommand.Connection?.State != ConnectionState.Open)
                    OpenConnection();

                if (contextData.UseTransaction)
                {
                    contextData.Transaction ??= contextData.Connection.BeginTransaction((System.Data.IsolationLevel)contextData.IsolationLevel);

                    innerCommand.Transaction = contextData.Transaction;
                }

                contextData.OnExecuting?.Invoke(new CommandEventArgs(innerCommand));

                if (useReader)
                    _command.Data.Reader = new DataReader(innerCommand.ExecuteReader());

                _queryAlreadyExecuted = true;
            }
        }

        /// <summary>
        /// Opens the database connection and triggers connection events.
        /// </summary>
        private void OpenConnection()
        {
            var connection = _command.Data.InnerCommand.Connection;
            var connectionEventArgs = new ConnectionEventArgs(connection);

            var contextData = _command.Data.Context.Data;
            contextData.OnConnectionOpening?.Invoke(connectionEventArgs);

            connection?.Open();

            contextData.OnConnectionOpened?.Invoke(connectionEventArgs);
        }

        /// <summary>
        /// Handles cleanup after query execution, closing reader and connection if needed.
        /// </summary>
        private void HandleQueryFinally()
        {
            if (!_command.Data.UseMultipleResultsets)
            {
                _command.Data.Reader?.Close();

                _command.ClosePrivateConnection();
            }
        }

        /// <summary>
        /// Handles exceptions during query execution, ensuring proper cleanup and error event notification.
        /// </summary>
        /// <param name="exception">The exception that occurred during execution.</param>
        private void HandleQueryException(Exception exception)
        {
            _command.Data.Reader?.Close();

            _command.ClosePrivateConnection();

            var contextData = _command.Data.Context.Data;
            if (contextData.UseTransaction)
                _command.Data.Context.CloseSharedConnection();

            contextData.OnError?.Invoke(new ErrorEventArgs(_command.Data.InnerCommand, exception));

            throw exception;
        }
    }
}
