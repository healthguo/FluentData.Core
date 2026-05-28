namespace FluentData
{
    /// <summary>
    /// Main database context class that provides the entry point for FluentData operations.
    /// Manages connection, transaction, and command lifecycle.
    /// </summary>
    public partial class DbContext : IDbContext
    {
        /// <summary>
        /// Gets the internal data object containing context configuration and state.
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
        /// Closes the shared connection if one exists, rolling back any active transaction.
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
        /// Disposes the context and closes any shared connection.
        /// </summary>
        public void Dispose()
        {
            CloseSharedConnection();
        }
    }
}
