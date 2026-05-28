namespace FluentData.Core
{
    /// <summary>
    /// Provides a shared fluent interface for configuring columns in both INSERT and UPDATE commands.
    /// Used by the <c>Fill</c> method to share column configuration logic between builders.
    /// </summary>
    /// <remarks>
    /// This helper interface centralizes column configuration so callers can reuse the same fill logic for both insert and update operations.
    /// </remarks>
    public interface IInsertUpdateBuilder
    {
        /// <summary>
        /// Gets the builder data containing command, columns, and table name.
        /// </summary>
        BuilderData Data { get; }

        /// <summary>
        /// Adds a column and its value to the command.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder"/> instance for method chaining.</returns>
        IInsertUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);

        /// <summary>
        /// Conditionally adds a column and its value to the command.
        /// </summary>
        /// <param name="condition">If true, the column is added; otherwise, it is skipped.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The value for the column.</param>
        /// <param name="parameterType">The database data type. Default is <see cref="DataTypes.Object"/> for automatic detection.</param>
        /// <param name="size">The maximum size of the parameter. Use 0 for default.</param>
        /// <returns>The current <see cref="IInsertUpdateBuilder"/> instance for method chaining.</returns>
        IInsertUpdateBuilder ColumnIf(bool condition, string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
    }
}
