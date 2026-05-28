using System.Data;

namespace FluentData.Core
{
    /// <summary>
    /// Represents a high-level FluentData database command.
    /// Supports building SQL, adding parameters, executing commands, and mapping query results.
    /// This interface composes execution, querying, parameter retrieval and disposal behaviors.
    /// </summary>
    /// <remarks>
    /// Implementations provide a fluent API for composing SQL statements and executing them against
    /// the configured <see cref="Context.DbContext"/>. Typical usage chains methods such as
    /// <c>Sql(...).Parameter(...).QueryMany&lt;T&gt;()</c>.
    /// </remarks>
    public interface IDbCommand : IExecute, IExecuteAsync, IExecuteReturnLastId, IQuery, IQueryAsync, IParameterValue, IDisposable
    {
        /// <summary>
        /// Gets the command data containing SQL, parameters, context and execution state.
        /// </summary>
        DbCommandData Data { get; }

        /// <summary>
        /// Adds a pre-configured ADO.NET parameter to the command.
        /// </summary>
        /// <param name="parameter">The <see cref="IDataParameter"/> to add. Cannot be null.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand Parameter(IDataParameter parameter);

        /// <summary>
        /// Adds a parameter with the specified name, value, and type to the command.
        /// </summary>
        /// <param name="name">The parameter name (with or without provider-specific prefix such as '@').</param>
        /// <param name="value">The parameter value. May be <c>null</c> to represent a database NULL.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="direction">The parameter direction. Default is <see cref="ParameterDirection.Input"/>.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);

        /// <summary>
        /// Adds an output parameter to the command. Use <see cref="IParameterValue.ParameterValue{TParameterType}(string)"/> to read after execution.
        /// </summary>
        /// <param name="name">The parameter name (with or without provider-specific prefix).</param>
        /// <param name="parameterType">The database data type of the output parameter.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand ParameterOut(string name, DataTypes parameterType, int size = 0);

        /// <summary>
        /// Adds multiple positional parameters to the command using auto-generated names (@0, @1, etc.).
        /// </summary>
        /// <param name="parameters">The parameter values to add.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand Parameters(params object[] parameters);

        /// <summary>
        /// Appends SQL text to the command.
        /// </summary>
        /// <param name="sql">The SQL text to append.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand Sql(string sql);

        /// <summary>
        /// Gets a reference to clear the accumulated SQL text.
        /// Use this to reset the command before building a new statement without creating a new IDbCommand instance.
        /// </summary>
        IDbCommand ClearSql { get; }

        /// <summary>
        /// Sets the command type (Text, StoredProcedure, or TableDirect).
        /// </summary>
        /// <param name="dbCommandType">The <see cref="DbCommandTypes"/> value.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand CommandType(DbCommandTypes dbCommandType);

        /// <summary>
        /// Configures whether the command should handle multiple result sets.
        /// </summary>
        /// <param name="useMultipleResultsets">True to enable multiple result set handling; false otherwise.</param>
        /// <returns>The current <see cref="IDbCommand"/> instance for method chaining.</returns>
        IDbCommand UseMultiResult(bool useMultipleResultsets);
    }
}
