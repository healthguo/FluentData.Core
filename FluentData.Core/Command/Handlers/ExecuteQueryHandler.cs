using System.Data;

namespace FluentData.Core
{
    internal class ExecuteQueryHandler
    {
        private readonly DbCommand _command;

        private bool _queryAlreadyExecuted;

        public ExecuteQueryHandler(DbCommand command)
        {
            _command = command;
        }

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

        private void OpenConnection()
        {
            var connection = _command.Data.InnerCommand.Connection;
            var connectionEventArgs = new ConnectionEventArgs(connection);

            var contextData = _command.Data.Context.Data;
            contextData.OnConnectionOpening?.Invoke(connectionEventArgs);

            connection?.Open();

            contextData.OnConnectionOpened?.Invoke(connectionEventArgs);
        }

        private void HandleQueryFinally()
        {
            if (!_command.Data.UseMultipleResultsets)
            {
                _command.Data.Reader?.Close();

                _command.ClosePrivateConnection();
            }
        }

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
