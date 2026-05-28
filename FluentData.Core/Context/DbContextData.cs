namespace FluentData.Core
{
    /// <summary>
    /// Contains configuration and state data for a <see cref="DbContext"/> instance.
    /// </summary>
    public class DbContextData
    {
        /// <summary>
        /// Gets or sets a value indicating whether transactions are enabled.
        /// </summary>
        public bool UseTransaction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a shared connection is used across commands.
        /// </summary>
        public bool UseSharedConnection { get; set; }

        /// <summary>
        /// Gets or sets the underlying ADO.NET <see cref="System.Data.IDbConnection"/>.
        /// </summary>
        public System.Data.IDbConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the ADO.NET <see cref="System.Data.Common.DbProviderFactory"/> for creating connections and commands.
        /// </summary>
        public System.Data.Common.DbProviderFactory AdoNetProvider { get; set; }

        /// <summary>
        /// Gets or sets the isolation level for transactions.
        /// </summary>
        public IsolationLevel IsolationLevel { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="System.Data.IDbTransaction"/>.
        /// </summary>
        public System.Data.IDbTransaction Transaction { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDbProvider"/> for database-specific operations.
        /// </summary>
        public IDbProvider FluentDataProvider { get; set; }

        /// <summary>
        /// Gets or sets the database connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IEntityFactory"/> for creating entity instances.
        /// </summary>
        public IEntityFactory EntityFactory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore errors during auto-mapping.
        /// </summary>
        public bool IgnoreIfAutoMapFails { get; set; }

        /// <summary>
        /// Gets or sets the command timeout in seconds.
        /// </summary>
        public int CommandTimeout { get; set; }

        /// <summary>
        /// Gets or sets the action invoked before a connection opens.
        /// </summary>
        public Action<ConnectionEventArgs> OnConnectionOpening { get; set; }

        /// <summary>
        /// Gets or sets the action invoked after a connection opens.
        /// </summary>
        public Action<ConnectionEventArgs> OnConnectionOpened { get; set; }

        /// <summary>
        /// Gets or sets the action invoked after a connection closes.
        /// </summary>
        public Action<ConnectionEventArgs> OnConnectionClosed { get; set; }

        /// <summary>
        /// Gets or sets the action invoked before a command executes.
        /// </summary>
        public Action<CommandEventArgs> OnExecuting { get; set; }

        /// <summary>
        /// Gets or sets the action invoked after a command executes.
        /// </summary>
        public Action<CommandEventArgs> OnExecuted { get; set; }

        /// <summary>
        /// Gets or sets the action invoked when an error occurs during command execution.
        /// </summary>
        public Action<ErrorEventArgs> OnError { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="DbContextData"/> with default values.
        /// </summary>
        public DbContextData()
        {
            IgnoreIfAutoMapFails = false;
            UseTransaction = false;
            IsolationLevel = IsolationLevel.ReadCommitted;
            EntityFactory = new EntityFactory();
            CommandTimeout = int.MinValue;
        }
    }
}
