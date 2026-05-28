using System.Data.Common;

namespace FluentData.Core
{
    /// <summary>
    /// Contract that database-specific provider implementations must satisfy.
    /// Provider implementations encapsulate differences between database engines such as
    /// SQL generation, parameter naming, identity/returning behavior, and supported features.
    /// </summary>
    /// <remarks>
    /// Typical implementations include SQL Server, MySQL, Oracle, PostgreSQL providers.
    /// The library uses this abstraction to generate provider-specific SQL and to adapt
    /// runtime behaviors (e.g., whether multiple result sets or output parameters are supported).
    /// </remarks>
    public interface IDbProvider
    {
        /// <summary>
        /// Gets the logical provider name (for example, "SqlServer", "MySql", "PostgreSql").
        /// </summary>
        /// <value>A short provider identifier used by callers and configuration.</value>
        string ProviderName { get; }

        /// <summary>
        /// Indicates whether the provider supports reading multiple result sets from a single command.
        /// </summary>
        /// <remarks>
        /// When <c>true</c>, the library may enable multi-resultset handling APIs.
        /// </remarks>
        bool SupportsMultipleResultsets { get; }

        /// <summary>
        /// Indicates whether the provider supports executing multiple SQL statements in a single command.
        /// </summary>
        /// <remarks>
        /// This affects APIs that batch multiple queries together; some providers (e.g., SQLite)
        /// may not allow multiple statements separated by semicolons.
        /// </remarks>
        bool SupportsMultipleQueries { get; }

        /// <summary>
        /// Indicates whether the provider supports output parameters for commands and stored procedures.
        /// </summary>
        /// <remarks>
        /// If <c>false</c>, output parameter related APIs should not be used or will throw at runtime.
        /// </remarks>
        bool SupportsOutputParameters { get; }

        /// <summary>
        /// Indicates whether the provider supports executing stored procedures.
        /// </summary>
        bool SupportsStoredProcedures { get; }

        /// <summary>
        /// Indicates whether the provider requires explicitly specifying the identity/serial column name
        /// when returning the last inserted id. Some providers return the last inserted value automatically,
        /// while others require additional syntax or explicit column names.
        /// </summary>
        bool RequiresIdentityColumn { get; }

        /// <summary>
        /// Formats the parameter name according to the database provider's convention.
        /// </summary>
        /// <param name="parameterName">The raw parameter name.</param>
        /// <returns>The formatted parameter name (e.g., "@Name" for SQL Server, ":Name" for Oracle).</returns>
        string GetParameterName(string parameterName);

        /// <summary>
        /// Gets the alias format for SELECT builder column names.
        /// </summary>
        /// <param name="name">The original column name.</param>
        /// <param name="alias">The alias to apply.</param>
        /// <returns>The formatted column name with alias.</returns>
        string GetSelectBuilderAlias(string name, string alias);

        /// <summary>
        /// Generates the SQL statement for a SELECT builder.
        /// </summary>
        /// <param name="data">The select builder data containing SQL clauses.</param>
        /// <returns>The generated SQL statement.</returns>
        string GetSqlForSelectBuilder(SelectBuilderData data);

        /// <summary>
        /// Generates the SQL statement for an INSERT builder.
        /// </summary>
        /// <param name="data">The builder data containing columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        string GetSqlForInsertBuilder(BuilderData data);

        /// <summary>
        /// Generates the SQL statement for an UPDATE builder.
        /// </summary>
        /// <param name="data">The builder data containing columns, WHERE columns, and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        string GetSqlForUpdateBuilder(BuilderData data);

        /// <summary>
        /// Generates the SQL statement for a DELETE builder.
        /// </summary>
        /// <param name="data">The builder data containing WHERE columns and table name.</param>
        /// <returns>The generated SQL statement.</returns>
        string GetSqlForDeleteBuilder(BuilderData data);

        /// <summary>
        /// Generates the SQL statement for a stored procedure builder.
        /// </summary>
        /// <param name="data">The builder data containing parameters and procedure name.</param>
        /// <returns>The generated SQL statement.</returns>
        string GetSqlForStoredProcedureBuilder(BuilderData data);

        /// <summary>
        /// Maps a CLR type to the corresponding database data type.
        /// </summary>
        /// <param name="clrType">The CLR type to map.</param>
        /// <returns>The corresponding <see cref="DataTypes"/> value.</returns>
        DataTypes GetDbTypeForClrType(Type clrType);

        /// <summary>
        /// Executes an INSERT command and returns the last inserted identity value.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="command">The database command to execute.</param>
        /// <param name="identityColumnName">Optional identity column name for databases requiring explicit specification.</param>
        /// <returns>The last inserted identity value.</returns>
        object ExecuteReturnLastId<T>(IDbCommand command, string identityColumnName);

        /// <summary>
        /// Called before a command is executed, allowing provider-specific command configuration.
        /// </summary>
        /// <param name="command">The command about to be executed.</param>
        void OnCommandExecuting(IDbCommand command);

        /// <summary>
        /// Escapes a column name to handle reserved keywords or special characters.
        /// </summary>
        /// <param name="name">The column name to escape.</param>
        /// <returns>The escaped column name (e.g., "[Name]" for SQL Server, "`Name`" for MySQL).</returns>
        string EscapeColumnName(string name);

        /// <summary>
        /// Registers a DbProviderFactory for the provider, enabling dynamic provider loading.
        /// </summary>
        /// <param name="providerName">Optional provider name to register. If null, uses the default.</param>
        void RegisterDbProviderFactory(string providerName = null);

        /// <summary>
        /// Gets the registered DbProviderFactory for creating database connections and commands.
        /// </summary>
        /// <param name="providerName">Optional provider name. If null, uses the default.</param>
        /// <returns>The <see cref="DbProviderFactory"/> instance.</returns>
        DbProviderFactory GetDbProviderFactory(string providerName = null);

        /// <summary>
        /// Unregisters a previously registered DbProviderFactory.
        /// </summary>
        /// <param name="providerName">Optional provider name to unregister. If null, unregisters the default.</param>
        /// <returns>True if the provider was successfully unregistered; false otherwise.</returns>
        bool UnregisterDbProviderFactory(string providerName = null);
    }
}
