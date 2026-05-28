namespace FluentData.Core
{
    /// <summary>
    /// Fluent builder for composing and executing UPDATE statements.
    /// </summary>
    /// <remarks>
    /// Use this builder to specify columns and WHERE conditions for UPDATE operations.
    /// Methods are chainable and the final operation should be executed with <see cref="IExecute.Execute"/>
    /// or <see cref="IExecuteAsync.ExecuteAsync"/>.
    /// </remarks>
    public interface IUpdateBuilder : IExecute, IExecuteAsync
    {
        /// <summary>
        /// Gets the builder data containing command, columns, WHERE columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        IUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        IUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        IUpdateBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a WHERE clause condition to the UPDATE command.
        /// </summary>
        /// <param name="condition">If true, the WHERE condition is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The column name for the WHERE condition. Must not be null or empty.</param>
        /// <param name="value">The value to compare against. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        IUpdateBuilder WhereIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the UPDATE command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder"/>.</param>
        /// <returns>The current <see cref="IUpdateBuilder"/> instance for method chaining.</returns>
        IUpdateBuilder Fill(Action<IInsertUpdateBuilder> fillMethod);
    }
}
