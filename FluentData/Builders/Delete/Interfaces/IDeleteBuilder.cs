namespace FluentData
{
    /// <summary>
    /// Provides a fluent interface for building and executing DELETE commands.
    /// </summary>
    /// <remarks>
    /// Use this builder to add WHERE conditions and execute a DELETE statement.
    /// Chain conditions with <see cref="Where(string, object, DataTypes, int)"/> and finish with
    /// <see cref="IExecute.Execute"/> or <see cref="IExecuteAsync.ExecuteAsync"/>.
    /// </remarks>
    public interface IDeleteBuilder : IExecute, IExecuteAsync
    {
        /// <summary>
        /// Gets the builder data containing command, WHERE columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Adds a WHERE clause condition to the DELETE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder"/> instance for method chaining.</returns>
        IDeleteBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition to the DELETE command.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IDeleteBuilder"/> instance for method chaining.</returns>
        IDeleteBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}