using System;

namespace FluentData
{
    /// <summary>
    /// Defines the contract for database provider implementations.
    /// Each provider handles database-specific SQL generation, parameter naming, and type mapping.
    /// </summary>
    public interface IDbProvider
    {
        /// <summary>
        /// Gets the name of the database provider (e.g., "SqlServer", "MySql", "PostgreSql").
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Gets a value indicating whether the provider supports multiple result sets in a single query.
        /// </summary>
        bool SupportsMultipleResultsets { get; }

        /// <summary>
        /// Gets a value indicating whether the provider supports multiple queries in a single command.
        /// </summary>
        bool SupportsMultipleQueries { get; }

        /// <summary>
        /// Gets a value indicating whether the provider supports output parameters.
        /// </summary>
        bool SupportsOutputParameters { get; }

        /// <summary>
        /// Gets a value indicating whether the provider supports stored procedures.
        /// </summary>
        bool SupportsStoredProcedures { get; }

        /// <summary>
        /// Gets a value indicating whether the provider requires an explicit identity column specification for INSERT operations.
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
    }
}
