namespace FluentData.Core
{
    /// <summary>
    /// Represents a database context for managing connections, transactions, and command execution.
    /// Provides the entry point for building and executing database commands using FluentData's fluent API.
    /// </summary>
    public partial class DbContext : IDbContext
    {
        /// <summary>
        /// Gets the context data containing configuration and state information.
        /// </summary>
        public DbContextData Data { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="DbContext"/> with default configuration.
        /// </summary>
        public DbContext()
        {
            Data = new DbContextData();
        }

        /// <summary>
        /// Closes the shared connection and rolls back any pending transaction.
        /// </summary>
        internal void CloseSharedConnection()
        {
            if (Data.Connection == null)
                return;

            if (Data.UseTransaction
                && Data.Transaction != null)
                Rollback();

            Data.Connection.Close();

            Data.OnConnectionClosed?.Invoke(new ConnectionEventArgs(Data.Connection));
        }

        /// <summary>
        /// Releases resources and closes the shared connection.
        /// </summary>
        public void Dispose()
        {
            CloseSharedConnection();
        }
    }
}
