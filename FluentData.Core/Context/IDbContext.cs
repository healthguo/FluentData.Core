using Microsoft.Extensions.Configuration;
using System.Dynamic;

namespace FluentData.Core
{
    /// <summary>
    /// Represents the main database context for FluentData ORM operations.
    /// Provides methods for creating commands, builders, managing transactions, and configuring connection settings.
    /// </summary>
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Gets the context data containing connection, provider, and configuration information.
        /// </summary>
        DbContextData Data { get; }

        /// <summary>
        /// Configures whether to ignore mapping failures during auto-mapping.
        /// </summary>
        /// <param name="ignoreIfAutoMapFails">True to ignore mapping failures; false to throw exceptions on mismatch.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext IgnoreIfAutoMapFails(bool ignoreIfAutoMapFails);

        /// <summary>
        /// Configures whether to use transactions for database operations.
        /// </summary>
        /// <param name="useTransaction">True to enable transaction support; false for non-transactional operations.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext UseTransaction(bool useTransaction);

        /// <summary>
        /// Configures whether to use a shared connection across operations.
        /// </summary>
        /// <param name="useSharedConnection">True to reuse a single connection; false to open/close per operation.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext UseSharedConnection(bool useSharedConnection);

        /// <summary>
        /// Sets the command timeout in seconds.
        /// </summary>
        /// <param name="timeout">The timeout duration in seconds. Use 0 for default timeout.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext CommandTimeout(int timeout);

        /// <summary>
        /// Creates a raw SQL command with the specified SQL and parameters.
        /// </summary>
        /// <param name="sql">The SQL command text.</param>
        /// <param name="parameters">Optional positional parameters for the SQL command.</param>
        /// <returns>An <see cref="IDbCommand"/> instance for further configuration and execution.</returns>
        IDbCommand Sql(string sql, params object[] parameters);

        /// <summary>
        /// Gets the command for handling multiple result sets from a single query.
        /// </summary>
        IDbCommand MultiResultSql { get; }

        /// <summary>
        /// Creates a SELECT command builder for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type to map query results to.</typeparam>
        /// <param name="sql">The SELECT clause columns (e.g., "Id, Name").</param>
        /// <returns>An <see cref="ISelectBuilder{TEntity}"/> instance for building the SELECT query.</returns>
        ISelectBuilder<TEntity> Select<TEntity>(string sql);

        /// <summary>
        /// Creates an INSERT command builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <returns>An <see cref="IInsertBuilder"/> instance for building the INSERT command.</returns>
        IInsertBuilder Insert(string tableName);

        /// <summary>
        /// Creates a strongly-typed INSERT command builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to insert.</typeparam>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The entity instance containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilder{T}"/> instance for building the INSERT command.</returns>
        IInsertBuilder<T> Insert<T>(string tableName, T item);

        /// <summary>
        /// Creates a strongly-typed INSERT command builder, auto-detecting the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to insert.</typeparam>
        /// <param name="item">The entity instance containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilder{T}"/> instance for building the INSERT command.</returns>
        IInsertBuilder<T> Insert<T>(T item);

        /// <summary>
        /// Creates a dynamic INSERT command builder for the specified table and entity.
        /// </summary>
        /// <param name="tableName">The name of the table to insert into.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing values to insert.</param>
        /// <returns>An <see cref="IInsertBuilderDynamic"/> instance for building the INSERT command.</returns>
        IInsertBuilderDynamic Insert(string tableName, ExpandoObject item);

        /// <summary>
        /// Creates an UPDATE command builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <returns>An <see cref="IUpdateBuilder"/> instance for building the UPDATE command.</returns>
        IUpdateBuilder Update(string tableName);

        /// <summary>
        /// Creates a strongly-typed UPDATE command builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to update.</typeparam>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The entity instance containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilder{T}"/> instance for building the UPDATE command.</returns>
        IUpdateBuilder<T> Update<T>(string tableName, T item);

        /// <summary>
        /// Creates a strongly-typed UPDATE command builder, auto-detecting the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing values to update.</typeparam>
        /// <param name="item">The entity instance containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilder{T}"/> instance for building the UPDATE command.</returns>
        IUpdateBuilder<T> Update<T>(T item);

        /// <summary>
        /// Creates a dynamic UPDATE command builder for the specified table and entity.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing values to update.</param>
        /// <returns>An <see cref="IUpdateBuilderDynamic"/> instance for building the UPDATE command.</returns>
        IUpdateBuilderDynamic Update(string tableName, ExpandoObject item);

        /// <summary>
        /// Creates a DELETE command builder for the specified table.
        /// </summary>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <returns>An <see cref="IDeleteBuilder"/> instance for building the DELETE command.</returns>
        IDeleteBuilder Delete(string tableName);

        /// <summary>
        /// Creates a strongly-typed DELETE command builder for the specified table and entity.
        /// </summary>
        /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <param name="item">The entity instance containing WHERE clause values.</param>
        /// <returns>An <see cref="IDeleteBuilder{T}"/> instance for building the DELETE command.</returns>
        IDeleteBuilder<T> Delete<T>(string tableName, T item);

        /// <summary>
        /// Creates a strongly-typed DELETE command builder, auto-detecting the table name from the entity type.
        /// </summary>
        /// <typeparam name="T">The entity type containing WHERE clause values.</typeparam>
        /// <param name="item">The entity instance containing WHERE clause values.</param>
        /// <returns>An <see cref="IDeleteBuilder{T}"/> instance for building the DELETE command.</returns>
        IDeleteBuilder<T> Delete<T>(T item);

        /// <summary>
        /// Creates a stored procedure command builder.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <returns>An <see cref="IStoredProcedureBuilder"/> instance for building the stored procedure command.</returns>
        IStoredProcedureBuilder StoredProcedure(string storedProcedureName);

        /// <summary>
        /// Creates a strongly-typed stored procedure command builder.
        /// </summary>
        /// <typeparam name="T">The entity type containing stored procedure parameter values.</typeparam>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <param name="item">The entity instance containing parameter values.</param>
        /// <returns>An <see cref="IStoredProcedureBuilder{T}"/> instance for building the stored procedure command.</returns>
        IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item);

        /// <summary>
        /// Creates a dynamic stored procedure command builder.
        /// </summary>
        /// <param name="storedProcedureName">The name of the stored procedure.</param>
        /// <param name="item">The <see cref="ExpandoObject"/> containing parameter values.</param>
        /// <returns>An <see cref="IStoredProcedureBuilderDynamic"/> instance for building the stored procedure command.</returns>
        IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item);

        /// <summary>
        /// Sets the entity factory for creating entity instances during query mapping.
        /// </summary>
        /// <param name="entityFactory">The entity factory to use.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext EntityFactory(IEntityFactory entityFactory);

        /// <summary>
        /// Configures the connection string and database provider.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="fluentDataProvider">The FluentData database provider.</param>
        /// <param name="providerName">Optional ADO.NET provider name.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, string providerName = null);

        /// <summary>
        /// Configures the connection string using a FluentData provider and ADO.NET provider factory.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="fluentDataProvider">The FluentData database provider.</param>
        /// <param name="adoNetProviderFactory">The ADO.NET provider factory.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext ConnectionString(string connectionString, IDbProvider fluentDataProvider, System.Data.Common.DbProviderFactory adoNetProviderFactory);

        /// <summary>
        /// Loads a connection string by name from an <see cref="IConfiguration"/> instance.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance to read from.</param>
        /// <param name="connectionStringName">The name of the connection string in the configuration.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext ConnectionStringName(IConfiguration configuration, string connectionStringName, IDbProvider dbProvider);

        /// <summary>
        /// Loads a connection string by name from the application's default configuration file (e.g., app.config or web.config).
        /// </summary>
        /// <param name="connectionStringName">The name of the connection string in the configuration file.</param>
        /// <param name="dbProvider">The <see cref="IDbProvider"/> implementation for the target database.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext ConnectionStringNameFromConfigFile(string connectionStringName, IDbProvider dbProvider);

        /// <summary>
        /// Sets the transaction isolation level.
        /// </summary>
        /// <param name="isolationLevel">The isolation level for transactions.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext IsolationLevel(IsolationLevel isolationLevel);

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext Commit();

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext Rollback();

        /// <summary>
        /// Registers an event handler for the connection opening event.
        /// </summary>
        /// <param name="action">The action to execute when the connection is opening.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnConnectionOpening(Action<ConnectionEventArgs> action);

        /// <summary>
        /// Registers an event handler for the connection opened event.
        /// </summary>
        /// <param name="action">The action to execute after the connection is opened.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnConnectionOpened(Action<ConnectionEventArgs> action);

        /// <summary>
        /// Registers an event handler for the connection closed event.
        /// </summary>
        /// <param name="action">The action to execute after the connection is closed.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnConnectionClosed(Action<ConnectionEventArgs> action);

        /// <summary>
        /// Registers an event handler for the command executing event.
        /// </summary>
        /// <param name="action">The action to execute before a command is executed.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnExecuting(Action<CommandEventArgs> action);

        /// <summary>
        /// Registers an event handler for the command executed event.
        /// </summary>
        /// <param name="action">The action to execute after a command is executed.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnExecuted(Action<CommandEventArgs> action);

        /// <summary>
        /// Registers an event handler for the error event.
        /// </summary>
        /// <param name="action">The action to execute when an error occurs.</param>
        /// <returns>The current <see cref="IDbContext"/> instance for method chaining.</returns>
        IDbContext OnError(Action<ErrorEventArgs> action);
    }
}