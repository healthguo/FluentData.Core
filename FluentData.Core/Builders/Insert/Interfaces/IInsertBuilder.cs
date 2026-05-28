namespace FluentData.Core
{
    /// <summary>
    /// Provides a fluent interface for building and executing INSERT commands.
    /// </summary>
    /// <remarks>
    /// Use <see cref="Column(string, object, DataTypes, int)"/> to add columns and values, optionally calling
    /// <see cref="Fill(Action{IInsertUpdateBuilder})"/> to populate columns from a shared configuration.
    /// After configuration, call <see cref="IExecute.Execute"/>, <see cref="IExecuteAsync.ExecuteAsync"/>,
    /// or <see cref="IExecuteReturnLastId.ExecuteReturnLastId{T}(string)"/> as appropriate.
    /// </remarks>
    public interface IInsertBuilder : IExecute, IExecuteAsync, IExecuteReturnLastId
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="columnName">The name of the column. Must not be null or empty.</param>
        /// <param name="value">The value for the column. </param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        IInsertBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the INSERT command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        IInsertBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Fills the INSERT command columns using a shared configuration action.
        /// </summary>
        /// <param name="fillMethod">An action that configures columns via <see cref="IInsertUpdateBuilder"/>.</param>
        /// <returns>The current <see cref="IInsertBuilder"/> instance for method chaining.</returns>
        IInsertBuilder Fill(Action<IInsertUpdateBuilder> fillMethod);
    }
}
